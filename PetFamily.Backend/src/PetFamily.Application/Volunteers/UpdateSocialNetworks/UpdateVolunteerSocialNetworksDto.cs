using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksDto(IEnumerable<SocialNetworkDto> SocialNetworks);