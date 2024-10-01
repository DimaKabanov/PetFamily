using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.AddPetToVolunteer;

public class AddPetToVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<AddPetToVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<AddPetToVolunteerService> logger)
{
    public async Task<Result<Guid, ErrorList>> AddPet(
        AddPetToVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.NewId();
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