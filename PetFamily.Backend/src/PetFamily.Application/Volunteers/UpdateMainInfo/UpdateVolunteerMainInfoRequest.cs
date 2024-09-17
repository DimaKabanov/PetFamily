namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoRequest(
    Guid VolunteerId,
    UpdateVolunteerMainInfoDto Dto);