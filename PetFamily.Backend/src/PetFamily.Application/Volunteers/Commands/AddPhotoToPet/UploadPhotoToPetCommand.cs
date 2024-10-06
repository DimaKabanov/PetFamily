using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.Commands.AddPhotoToPet;

public record UploadPhotoToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadPhotoDto> Photos);