using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Commands.Pet.SetMainPhoto;

public record SetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string PhotoPath) : ICommand;