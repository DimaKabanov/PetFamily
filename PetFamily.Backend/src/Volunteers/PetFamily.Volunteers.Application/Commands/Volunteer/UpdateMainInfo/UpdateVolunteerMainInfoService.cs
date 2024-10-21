using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Application.Commands.Volunteer.Create;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateMainInfo;

public class UpdateVolunteerMainInfoService(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerMainInfoCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger) : ICommandService<Guid, UpdateVolunteerMainInfoCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerMainInfoCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var fullName = FullName.Create(
            command.FullName.Name,
            command.FullName.Surname,
            command.FullName.Patronymic).Value;
            
        var description = Description.Create(command.Description).Value;

        var experience = Experience.Create(command.Experience).Value;

        var phone = Phone.Create(command.Phone).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            description,
            experience,
            phone);

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Updated volunteer with id: {volunteerId}", volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}