namespace PetFamily.Application.Volunteers.AddPhotoToPet;

public record AddPhotoToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreatePhotoCommand> Photos);

public record CreatePhotoCommand(Stream Content, string PhotoName);