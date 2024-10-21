using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Pet.AddToVolunteer;

public class AddToVolunteerService(
    IVolunteersRepository volunteersRepository,
    IReadDbContext readDbContext,
    IValidator<AddToVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<AddToVolunteerService> logger) : ICommandService<Guid, AddToVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        AddToVolunteerCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
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
        
        var species = await readDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == command.SpeciesId, ct);
        if (species is null)
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();
        
        var breed = await readDbContext.Breeds
            .FirstOrDefaultAsync(b => b.SpeciesId == species.Id, ct);
        if (breed is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();
        
        var properties = new Property(SpeciesId.Create(command.SpeciesId), command.BreedId);
        
        var pet = new Domain.Models.Volunteers.Pets.Pet(
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
        
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation(
            "Added pet with id {petId} to volunteer with id {volunteerId}",
            petId,
            volunteerId);
        
        return petId.Value;
    }
}