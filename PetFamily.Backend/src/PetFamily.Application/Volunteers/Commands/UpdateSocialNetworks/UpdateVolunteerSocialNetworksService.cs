using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksService(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateVolunteerSocialNetworksCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger) : ICommandService<Guid, UpdateVolunteerSocialNetworksCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerSocialNetworksCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value)
            .ToList();

        volunteerResult.Value.UpdateSocialNetworks(socialNetworks);

        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Updated volunteer social networks with id: {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Value;
    }
}