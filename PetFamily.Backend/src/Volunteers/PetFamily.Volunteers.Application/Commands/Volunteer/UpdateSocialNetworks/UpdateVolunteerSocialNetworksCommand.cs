using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;