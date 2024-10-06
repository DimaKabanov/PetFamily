using PetFamily.Application.Dto;
using PetFamily.Domain.Enums;

namespace PetFamily.Application.Volunteers.Commands.AddPetToVolunteer;

public record AddPetToVolunteerCommand(
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
    IEnumerable<RequisiteDto> Requisites);