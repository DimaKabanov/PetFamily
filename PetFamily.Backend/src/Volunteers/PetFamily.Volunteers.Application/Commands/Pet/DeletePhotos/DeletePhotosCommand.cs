using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Commands.Pet.DeletePhotos;

public record DeletePhotosCommand(Guid VolunteerId, Guid PetId) : ICommand;