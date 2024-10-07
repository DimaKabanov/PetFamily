using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.Delete;

public class DeleteVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<DeleteVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteVolunteerService> logger) : ICommandService<Guid, DeleteVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Run(
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