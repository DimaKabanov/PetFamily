using PetFamily.Core.Dto;
using PetFamily.Volunteers.Application.Commands.Volunteer.UpdateSocialNetworks;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record UpdateVolunteerSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateVolunteerSocialNetworksCommand ToCommand(Guid volunteerId) => new(volunteerId, SocialNetworks);
};