namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateVolunteerRequisitesRequest(
    Guid VolunteerId,
    UpdateVolunteerRequisitesDto Dto);