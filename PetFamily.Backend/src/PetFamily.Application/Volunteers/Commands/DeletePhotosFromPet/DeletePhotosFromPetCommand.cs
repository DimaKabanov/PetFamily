using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.DeletePhotosFromPet;

public record DeletePhotosFromPetCommand(Guid VolunteerId, Guid PetId) : ICommand;