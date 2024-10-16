using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto;
using PetFamily.Domain.Enums;

namespace PetFamily.Application.Volunteers.Commands.EditPet;

public record EditPetCommand(
    Guid VolunteerId,
    Guid PetId,
    Guid SpeciesId,
    Guid BreedId,
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
    IEnumerable<RequisiteDto> Requisites) : ICommand;