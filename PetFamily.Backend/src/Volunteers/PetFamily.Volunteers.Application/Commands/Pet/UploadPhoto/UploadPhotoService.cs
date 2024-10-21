using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.Core.PhotoProvider;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Pet.UploadPhoto;

public class UploadPhotoService(
    IVolunteersRepository volunteersRepository,
    IPhotoProvider photoProvider,
    IValidator<UploadPhotoCommand> validator,
    IUnitOfWork unitOfWork,
    IMessageQueue<IEnumerable<PhotoInfo>> messageQueue,
    ILogger<UploadPhotoService> logger) : ICommandService<Guid, UploadPhotoCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UploadPhotoCommand command,
        CancellationToken ct)
    {
        var transaction = await unitOfWork.BeginTransaction(ct);
        
        try
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
                return validationResult.ToErrorList();
            
            var volunteerId = VolunteerId.Create(command.VolunteerId);
        
            var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();
            
            List<PhotoData> photosData = [];
            foreach (var photo in command.Photos)
            {
                var extension = Path.GetExtension(photo.PhotoName);
                var photoPath = PhotoPath.Create(Guid.NewGuid(), extension);

                var photoData = new PhotoData(photo.Content, new PhotoInfo(photoPath.Value, Constants.PHOTO_BUCKET_NAME));
                photosData.Add(photoData);
            }
            
            var photoPathsResult = await photoProvider.UploadFiles(photosData, ct);
            if (photoPathsResult.IsFailure)
            {
                await messageQueue.WriteAsync(photosData.Select(p => p.Info), ct);

                return photoPathsResult.Error.ToErrorList();
            }

            var photos = photoPathsResult.Value
                .Select(path => new Photo(path, false))
                .ToList();
            
            petResult.Value.UpdatePhotos(photos);
            
            await unitOfWork.SaveChanges(ct);
            
            transaction.Commit();
            
            logger.LogInformation("Added photos to pet with id {petId}", petId);
            
            return petId.Value;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Can not add photos to pet - {id} in transaction", command.PetId);

            transaction.Rollback();

            return Error.Failure("pet.photo.failure", "Can not add photos to pet").ToErrorList();
        }
    }
}