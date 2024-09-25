using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.PhotoProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetService(
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger)
{
    private const string BUCKET_NAME = "photos";
    
    public async Task<Result<Guid, Error>> AddPet(
        AddPetCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        
        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
        
            var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error;

            var petId = PetId.NewId;
            var name = Name.Create(command.Name).Value;
            var description = Description.Create(command.Description).Value;

            var physicalProperty = PhysicalProperty.Create(
                command.PhysicalProperty.Color,
                command.PhysicalProperty.Health,
                command.PhysicalProperty.Weight,
                command.PhysicalProperty.Height).Value;

            var address = Address.Create(
                command.Address.Street,
                command.Address.Home,
                command.Address.Flat).Value;

            var phone = Phone.Create(command.Phone).Value;
            var dateOfBirth = DateOfBirth.Create(command.DateOfBirth).Value;
            var createdDate = CreatedDate.Create(command.CreatedDate).Value;
            
            var requisites = command.Requisites
                .Select(r => Requisite.Create(r.Name, r.Description).Value)
                .ToList();
            
            List<PhotoData> photosData = [];
            foreach (var photo in command.Photos)
            {
                var extension = Path.GetExtension(photo.PhotoName);
                var photoPath = PhotoPath.Create(Guid.NewGuid(), extension);

                var photoData = new PhotoData(photo.Content, photoPath.Value, BUCKET_NAME);
                photosData.Add(photoData);
            }
            
            var photos = photosData
                .Select(photo => photo.PhotoPath)
                .Select(path => new Photo(path, false))
                .ToList();
            
            var properties = new Property(SpeciesId.EmptyId, Guid.Empty);
            
            var pet = new Pet(
                petId,
                name,
                description,
                physicalProperty,
                address,
                phone,
                command.IsCastrated,
                dateOfBirth,
                command.IsVaccinated,
                command.AssistanceStatus,
                createdDate,
                requisites,
                photos,
                properties);

            volunteerResult.Value.AddPet(pet);
            
            await unitOfWork.SaveChanges(cancellationToken);
            
            var uploadResult = await fileProvider.UploadFiles(photosData, cancellationToken);
            
            if (uploadResult.IsFailure)
                return uploadResult.Error;
            
            transaction.Commit();
            
            logger.LogInformation(
                "Added pet with id {petId} to volunteer with id {volunteerId}",
                petId,
                volunteerId);
            
            return pet.Id.Id;
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