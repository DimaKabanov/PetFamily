using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites
);