using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.EditPet;

public class EditPetService(
    IVolunteersRepository volunteersRepository,
    IReadDbContext readDbContext,
    IValidator<EditPetCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<EditPetService> logger): ICommandService<Guid, EditPetCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(EditPetCommand command, CancellationToken ct)
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

        petResult.Value.EditPet(
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

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Edited pet with id {petId}", petId);

        return petId.Value;
    }
}