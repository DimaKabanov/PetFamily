using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;