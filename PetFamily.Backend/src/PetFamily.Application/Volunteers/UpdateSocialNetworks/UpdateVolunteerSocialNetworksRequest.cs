namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateVolunteerSocialNetworksRequest(
    Guid VolunteerId,
    UpdateVolunteerSocialNetworksDto Dto);