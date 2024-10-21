using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Application.Commands.Pet.UploadPhoto;

public record UploadPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadPhotoDto> Photos) : ICommand;