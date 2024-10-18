using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.HardDeletePet;

public record HardDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;