namespace PetFamily.Application.Volunteers.AddPhotoToPet;

public record AddPhotoToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreatePhotoDto> Photos);

public record CreatePhotoDto(Stream Content, string PhotoName);