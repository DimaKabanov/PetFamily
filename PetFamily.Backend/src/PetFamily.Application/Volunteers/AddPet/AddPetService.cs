using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FIleProvider;
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
    ILogger<CreateVolunteerService> logger)
{
    private const string BUCKET_NAME = "photo";
    
    public async Task<Result<Guid, Error>> AddPet(
        AddPetCommand command,
        CancellationToken cancellationToken)
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
        
        List<FileContent> photoContents = [];
        List<FilePath> photoPaths = [];
        foreach (var photo in command.Photos)
        {
            var extension = Path.GetExtension(photo.PhotoName);
            var photoPath = FilePath.Create(Guid.NewGuid(), extension);

            var photoContent = new FileContent(photo.Stream, photoPath.Value.Path);
            photoContents.Add(photoContent);
            photoPaths.Add(photoPath.Value);
        }

        var photoData = new FileData(photoContents, BUCKET_NAME);
        
        var uploadResult = await fileProvider.UploadFiles(photoData, cancellationToken);
        
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        var photos = photoPaths.Select(p => new Photo(p, false)).ToList();
        
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
        
        var result = await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation(
            "Added pet with id {petId} to volunteer with id {volunteerId}",
            petId,
            volunteerId);
        
        return result;
    }
}