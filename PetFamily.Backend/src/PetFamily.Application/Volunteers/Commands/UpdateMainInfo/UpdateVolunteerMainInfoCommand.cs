using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone);