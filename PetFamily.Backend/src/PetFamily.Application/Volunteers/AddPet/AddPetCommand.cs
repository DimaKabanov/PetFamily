using PetFamily.Application.Volunteers.DTO;
using PetFamily.Domain.Enums;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    string Description,
    PetPhysicalPropertyDto PhysicalProperty,
    PetAddressDto Address,
    string Phone,
    bool IsCastrated,
    DateOnly DateOfBirth,
    bool IsVaccinated,
    AssistanceStatus AssistanceStatus,
    DateTime CreatedDate,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<CreatePhotoDto> Photos);

public record CreatePhotoDto(Stream Content, string PhotoName);