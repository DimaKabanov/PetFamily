using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone) : ICommand;