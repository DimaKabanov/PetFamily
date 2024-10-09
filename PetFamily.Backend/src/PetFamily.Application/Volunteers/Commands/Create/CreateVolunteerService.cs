using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.Create;

public class CreateVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger) : ICommandService<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Run(
        CreateVolunteerCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
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

        await volunteersRepository.Add(volunteer, ct);
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Create volunteer with id: {volunteerId}", volunteerId);

        return volunteerId.Value;
    }
}