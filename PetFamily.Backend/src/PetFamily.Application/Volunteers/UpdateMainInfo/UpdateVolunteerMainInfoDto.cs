using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone);