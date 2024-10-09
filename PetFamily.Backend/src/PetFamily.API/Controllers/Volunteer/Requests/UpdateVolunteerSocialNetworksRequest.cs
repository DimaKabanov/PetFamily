using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record UpdateVolunteerSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateVolunteerSocialNetworksCommand ToCommand(Guid volunteerId) => new(volunteerId, SocialNetworks);
};