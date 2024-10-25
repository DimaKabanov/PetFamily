using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Application.Commands.Volunteer.Create;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateSocialNetworks;

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