using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.Create;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites
)
{
    public CreateVolunteerCommand ToCommand() => 
        new(FullName, Description, Experience, Phone, SocialNetworks, Requisites);
};