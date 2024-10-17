using PetFamily.Application.Abstractions;
using PetFamily.Domain.Enums;

namespace PetFamily.Application.Volunteers.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    AssistanceStatus AssistanceStatus) : ICommand;