using PetFamily.Core.Dto;
using PetFamily.Volunteers.Application.Commands.Volunteer.Create;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

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