using PetFamily.Application.Volunteers.DTO;
using PetFamily.Domain.Enums;

namespace PetFamily.Application.Volunteers.AddPetToVolunteer;

public record AddPetToVolunteerDto(
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
    IEnumerable<RequisiteDto> Requisites);