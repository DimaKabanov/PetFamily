using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.Delete;

public class DeleteVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<DeleteVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteVolunteerService> logger) : ICommandService<Guid, DeleteVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.Delete();
        
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Deleted volunteer with id: {volunteerId}", volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}