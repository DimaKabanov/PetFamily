using PetFamily.Core.Dto;
using PetFamily.Volunteers.Application.Commands.Volunteer.UpdateRequisites;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record UpdateVolunteerRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateVolunteerRequisitesCommand ToCommand(Guid volunteerId) => new(volunteerId, Requisites);
};