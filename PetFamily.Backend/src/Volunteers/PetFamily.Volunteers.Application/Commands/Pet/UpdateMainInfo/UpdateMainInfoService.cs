using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Pet.UpdateMainInfo;

public class UpdateMainInfoService(
    IVolunteersRepository volunteersRepository,
    IReadDbContext readDbContext,
    IValidator<UpdateMainIngoCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<UpdateMainInfoService> logger): ICommandService<Guid, UpdateMainIngoCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(UpdateMainIngoCommand mainIngoCommand, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(mainIngoCommand, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(mainIngoCommand.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(mainIngoCommand.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var name = Name.Create(mainIngoCommand.Name).Value;
        var description = Description.Create(mainIngoCommand.Description).Value;

        var physicalProperty = PhysicalProperty.Create(
            mainIngoCommand.PhysicalProperty.Color,
            mainIngoCommand.PhysicalProperty.Health,
            mainIngoCommand.PhysicalProperty.Weight,
            mainIngoCommand.PhysicalProperty.Height).Value;

        var address = Address.Create(
            mainIngoCommand.Address.Street,
            mainIngoCommand.Address.Home,
            mainIngoCommand.Address.Flat).Value;

        var phone = Phone.Create(mainIngoCommand.Phone).Value;
        var dateOfBirth = DateOfBirth.Create(mainIngoCommand.DateOfBirth).Value;
        var createdDate = CreatedDate.Create(mainIngoCommand.CreatedDate).Value;

        var requisites = mainIngoCommand.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        var species = await readDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == mainIngoCommand.SpeciesId, ct);
        if (species is null)
            return Errors.General.NotFound(mainIngoCommand.SpeciesId).ToErrorList();

        var breed = await readDbContext.Breeds
            .FirstOrDefaultAsync(b => b.SpeciesId == species.Id, ct);
        if (breed is null)
            return Errors.General.NotFound(mainIngoCommand.BreedId).ToErrorList();

        var properties = new Property(SpeciesId.Create(mainIngoCommand.SpeciesId), mainIngoCommand.BreedId);

        petResult.Value.UpdatePet(
            name,
            description,
            physicalProperty,
            address,
            phone,
            mainIngoCommand.IsCastrated,
            dateOfBirth,
            mainIngoCommand.IsVaccinated,
            mainIngoCommand.AssistanceStatus,
            createdDate,
            requisites,
            properties);

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Edited pet with id {petId}", petId);

        return petId.Value;
    }
}