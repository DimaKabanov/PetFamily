using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateVolunteerRequisitesDto(IEnumerable<RequisiteDto> Requisites);