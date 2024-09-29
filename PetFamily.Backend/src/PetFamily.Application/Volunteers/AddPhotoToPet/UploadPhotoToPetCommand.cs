using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.AddPhotoToPet;

public record UploadPhotoToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadPhotoDto> Photos);