using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.Commands.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisiteDto> Requisites);