using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<DeleteVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteVolunteerService> logger)
{
    public async Task<Result<Guid, ErrorList>> Delete(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.Delete();
        
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation("Deleted volunteer with id: {volunteerId}", volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}