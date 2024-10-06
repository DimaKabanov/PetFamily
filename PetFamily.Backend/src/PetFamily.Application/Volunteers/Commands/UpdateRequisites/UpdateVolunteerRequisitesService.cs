using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.UpdateRequisites;

public class UpdateVolunteerRequisitesService(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerRequisitesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, ErrorList>> Update(
        UpdateVolunteerRequisitesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        volunteerResult.Value.UpdateRequisites(requisites);

        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation("Updated volunteer requisites with id: {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Value;
    }
}