using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisiteDto> Requisites);