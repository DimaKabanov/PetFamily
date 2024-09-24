using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksService(
    IVolunteersRepository volunteersRepository,
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
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value);

        var socialNetworkList = new ValueObjectList<SocialNetwork>(socialNetworks);

        volunteerResult.Value.UpdateSocialNetworks(socialNetworkList);
        
        var result = await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Updated volunteer social networks with id: {volunteerId}", volunteerId);
        
        return result;
    }
}