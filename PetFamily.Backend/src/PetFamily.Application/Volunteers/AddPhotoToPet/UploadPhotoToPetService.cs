using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.PhotoProvider;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPhotoToPet;

public class UploadPhotoToPetService(
    IVolunteersRepository volunteersRepository,
    IPhotoProvider photoProvider,
    IValidator<UploadPhotoToPetCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<UploadPhotoToPetService> logger)
{
    private const string BUCKET_NAME = "photos";
    
    public async Task<Result<Guid, ErrorList>> UploadPhoto(
        UploadPhotoToPetCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        
        try
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorList();
            
            var volunteerId = VolunteerId.Create(command.VolunteerId);
        
            var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
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

                var photoData = new PhotoData(photo.Content, photoPath.Value, BUCKET_NAME);
                photosData.Add(photoData);
            }
            
            var photoPathsResult = await photoProvider.UploadFiles(photosData, cancellationToken);
            if (photoPathsResult.IsFailure)
                return photoPathsResult.Error.ToErrorList();
            
            var photos = photoPathsResult.Value
                .Select(path => new Photo(path, false))
                .ToList();
            
            petResult.Value.UpdatePhotos(photos);
            
            await unitOfWork.SaveChanges(cancellationToken);
            
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