using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Volunteers.Domain.Enums;

namespace PetFamily.Volunteers.Application.Commands.Pet.AddToVolunteer;

public record AddToVolunteerCommand(
    Guid VolunteerId,
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