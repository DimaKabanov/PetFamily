using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Volunteers.Domain.Enums;

namespace PetFamily.Volunteers.Application.Commands.Pet.UpdateStatus;

public record UpdateStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    AssistanceStatus AssistanceStatus) : ICommand;