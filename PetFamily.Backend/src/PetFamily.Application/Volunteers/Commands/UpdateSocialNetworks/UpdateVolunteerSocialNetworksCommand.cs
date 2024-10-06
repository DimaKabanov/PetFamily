using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks);