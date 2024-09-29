using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record UpdateVolunteerMainInfoRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone)
{
    public UpdateVolunteerMainInfoCommand ToCommand(Guid volunteerId) => 
        new(volunteerId, FullName, Description, Experience, Phone);
};