using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.Create;

public class CreateVolunteerService(
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger) : ICommandService<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
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
        
        var volunteer = new Domain.Volunteer(
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