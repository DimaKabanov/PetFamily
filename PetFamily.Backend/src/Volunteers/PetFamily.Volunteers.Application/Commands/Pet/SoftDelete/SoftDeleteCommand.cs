using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Commands.Pet.SoftDelete;

public record SoftDeleteCommand(Guid VolunteerId, Guid PetId) : ICommand;