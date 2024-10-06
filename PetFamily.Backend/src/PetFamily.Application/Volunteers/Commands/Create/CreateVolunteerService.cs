using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.Create;

public class CreateVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> validator,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, ErrorList>> Create(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(
            command.FullName.Name,
            command.FullName.Surname,
            command.FullName.Patronymic).Value;
            
        var description = Description.Create(command.Description).Value;

        var experience = Experience.Create(command.Experience).Value;

        var phone = Phone.Create(command.Phone).Value;

        var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value)
            .ToList();

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();
        
        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            experience,
            phone,
            socialNetworks,
            requisites);

        await volunteersRepository.Add(volunteer, cancellationToken);
        
        logger.LogInformation("Create volunteer with id: {volunteerId}", volunteerId);

        return volunteerId.Value;
    }
}