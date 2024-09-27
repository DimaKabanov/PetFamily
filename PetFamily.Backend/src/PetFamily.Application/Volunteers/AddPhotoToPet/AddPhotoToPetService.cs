using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.PhotoProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPhotoToPet;

public class AddPhotoToPetService(
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IUnitOfWork unitOfWork,
    ILogger<AddPhotoToPetService> logger)
{
    private const string BUCKET_NAME = "photos";
    
    public async Task<Result<Guid, Error>> AddPhoto(
        AddPhotoToPetCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        
        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
        
            var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error;

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;
            
            List<PhotoData> photosData = [];
            foreach (var photo in command.Photos)
            {
                var extension = Path.GetExtension(photo.PhotoName);
                var photoPath = PhotoPath.Create(Guid.NewGuid(), extension);

                var photoData = new PhotoData(photo.Content, photoPath.Value, BUCKET_NAME);
                photosData.Add(photoData);
            }
            
            var uploadResult = await fileProvider.UploadFiles(photosData, cancellationToken);
            if (uploadResult.IsFailure)
                return uploadResult.Error;
            
            var photos = photosData
                .Select(photo => photo.PhotoPath)
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
            logger.LogError(e,
                "Can not add pet to volunteer - {id} in transaction", 
                command.VolunteerId);

            transaction.Rollback();

            return Error.Failure("volunteer.pet.failure", "Can not add pet to volunteer");
        }
    }
}