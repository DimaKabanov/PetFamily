using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Contracts;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Pet.UpdateMainInfo;

public class UpdateMainInfoService(
    ISpeciesContract speciesContract,
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateMainInfoCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<UpdateMainInfoService> logger): ICommandService<Guid, UpdateMainInfoCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(UpdateMainInfoCommand mainInfoCommand, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(mainInfoCommand, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(mainInfoCommand.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(mainInfoCommand.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var name = Name.Create(mainInfoCommand.Name).Value;
        var description = Description.Create(mainInfoCommand.Description).Value;

        var physicalProperty = PhysicalProperty.Create(
            mainInfoCommand.PhysicalProperty.Color,
            mainInfoCommand.PhysicalProperty.Health,
            mainInfoCommand.PhysicalProperty.Weight,
            mainInfoCommand.PhysicalProperty.Height).Value;

        var address = Address.Create(
            mainInfoCommand.Address.Street,
            mainInfoCommand.Address.Home,
            mainInfoCommand.Address.Flat).Value;

        var phone = Phone.Create(mainInfoCommand.Phone).Value;
        var dateOfBirth = DateOfBirth.Create(mainInfoCommand.DateOfBirth).Value;
        var createdDate = CreatedDate.Create(mainInfoCommand.CreatedDate).Value;

        var requisites = mainInfoCommand.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        var speciesResult = await speciesContract.GetSpeciesById(mainInfoCommand.SpeciesId, ct);
        if (speciesResult.IsSuccess)
            return Errors.General.NotFound(mainInfoCommand.SpeciesId).ToErrorList();

        var breedResult = await speciesContract.GetBreedBySpeciesId(speciesResult.Value.Id, ct);
        if (breedResult.IsSuccess)
            return Errors.General.NotFound(mainInfoCommand.BreedId).ToErrorList();

        var properties = new Property(SpeciesId.Create(mainInfoCommand.SpeciesId), mainInfoCommand.BreedId);

        petResult.Value.UpdatePet(
            name,
            description,
            physicalProperty,
            address,
            phone,
            mainInfoCommand.IsCastrated,
            dateOfBirth,
            mainInfoCommand.IsVaccinated,
            mainInfoCommand.AssistanceStatus,
            createdDate,
            requisites,
            properties);

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Edited pet with id {petId}", petId);

        return petId.Value;
    }
}