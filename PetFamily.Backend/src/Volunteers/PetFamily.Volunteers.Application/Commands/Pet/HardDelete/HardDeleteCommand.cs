using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Commands.Pet.HardDelete;

public record HardDeleteCommand(Guid VolunteerId, Guid PetId) : ICommand;