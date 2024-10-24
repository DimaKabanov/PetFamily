using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.SoftDeletePet;

public class SoftDeletePetService(
    IVolunteersRepository volunteersRepository,
    IValidator<SoftDeletePetCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<SoftDeletePetService> logger) : ICommandService<Guid, SoftDeletePetCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeletePetCommand command,
        CancellationToken ct)
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
        
        petResult.Value.Delete();
        
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Deleted pet with id: {petId}", petId);

        return petId.Value;
    }
}