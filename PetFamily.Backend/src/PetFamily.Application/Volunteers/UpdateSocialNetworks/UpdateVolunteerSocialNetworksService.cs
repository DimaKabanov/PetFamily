using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksService(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, Error>> Update(
        UpdateVolunteerSocialNetworksRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var socialNetworks = request.Dto.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value)
            .ToList();

        volunteerResult.Value.UpdateSocialNetworks(socialNetworks);

        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation("Updated volunteer social networks with id: {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Value;
    }
}