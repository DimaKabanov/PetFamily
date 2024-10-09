using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
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