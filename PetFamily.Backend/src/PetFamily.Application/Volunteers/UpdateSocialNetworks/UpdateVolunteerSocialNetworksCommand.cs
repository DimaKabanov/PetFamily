using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks);