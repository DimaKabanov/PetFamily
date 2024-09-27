using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.AddPetToVolunteer;

public class AddPetToVolunteerService(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    ILogger<AddPetToVolunteerService> logger)
{
    public async Task<Result<Guid, Error>> AddPet(
        AddPetToVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var petId = PetId.NewId();
        var name = Name.Create(request.Dto.Name).Value;
        var description = Description.Create(request.Dto.Description).Value;

        var physicalProperty = PhysicalProperty.Create(
            request.Dto.PhysicalProperty.Color,
            request.Dto.PhysicalProperty.Health,
            request.Dto.PhysicalProperty.Weight,
            request.Dto.PhysicalProperty.Height).Value;

        var address = Address.Create(
            request.Dto.Address.Street,
            request.Dto.Address.Home,
            request.Dto.Address.Flat).Value;

        var phone = Phone.Create(request.Dto.Phone).Value;
        var dateOfBirth = DateOfBirth.Create(request.Dto.DateOfBirth).Value;
        var createdDate = CreatedDate.Create(request.Dto.CreatedDate).Value;
        
        var requisites = request.Dto.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();
        
        var properties = new Property(SpeciesId.EmptyId, Guid.Empty);
        
        var pet = new Pet(
            petId,
            name,
            description,
            physicalProperty,
            address,
            phone,
            request.Dto.IsCastrated,
            dateOfBirth,
            request.Dto.IsVaccinated,
            request.Dto.AssistanceStatus,
            createdDate,
            requisites,
            properties);

        volunteerResult.Value.AddPet(pet);
        
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation(
            "Added pet with id {petId} to volunteer with id {volunteerId}",
            petId,
            volunteerId);
        
        return petId.Value;
    }
}