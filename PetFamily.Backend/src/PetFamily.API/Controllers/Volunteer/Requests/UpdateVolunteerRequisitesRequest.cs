using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record UpdateVolunteerRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateVolunteerRequisitesCommand ToCommand(Guid volunteerId) => new(volunteerId, Requisites);
};