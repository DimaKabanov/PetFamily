using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoService(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerMainInfoCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, ErrorList>> Update(
        UpdateVolunteerMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
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

        await unitOfWork.SaveChanges(cancellationToken);

        logger.LogInformation("Updated volunteer with id: {volunteerId}", volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}