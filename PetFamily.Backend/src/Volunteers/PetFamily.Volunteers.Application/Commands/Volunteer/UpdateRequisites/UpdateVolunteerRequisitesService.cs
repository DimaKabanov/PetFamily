using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Application.Commands.Volunteer.Create;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateRequisites;

public class UpdateVolunteerRequisitesService(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerRequisitesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger) : ICommandService<Guid, UpdateVolunteerRequisitesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerRequisitesCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        volunteerResult.Value.UpdateRequisites(requisites);

        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Updated volunteer requisites with id: {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Value;
    }
}