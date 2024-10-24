using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Volunteers.Application.Commands.Pet.UpdateStatus;

public class UpdateStatusService(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateStatusCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<UpdateStatusService> logger) : ICommandService<Guid, UpdateStatusCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(UpdateStatusCommand command, CancellationToken ct)
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

        petResult.Value.UpdateAssistanceStatus(command.AssistanceStatus);

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Updated AssistanceStatus for pet with id {petId}", petId);

        return petId.Value;
    }
}