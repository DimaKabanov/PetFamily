using PetFamily.Core.Dto;
using PetFamily.Volunteers.Application.Commands.Volunteer.UpdateMainInfo;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record UpdateVolunteerMainInfoRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone)
{
    public UpdateVolunteerMainInfoCommand ToCommand(Guid volunteerId) => 
        new(volunteerId, FullName, Description, Experience, Phone);
};