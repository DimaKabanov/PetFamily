using PetFamily.Core.Dto;
using PetFamily.Volunteers.Application.Commands.Pet.UpdateMainInfo;
using PetFamily.Volunteers.Domain.Enums;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record UpdatePetRequest(
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
    IEnumerable<RequisiteDto> Requisites)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId, Guid petId) => 
        new(
            volunteerId,
            petId,
            SpeciesId,
            BreedId,
            Name, 
            Description, 
            PhysicalProperty, 
            Address, 
            Phone,
            IsCastrated,
            DateOfBirth,
            IsVaccinated,
            AssistanceStatus,
            CreatedDate,
            Requisites);
};
    