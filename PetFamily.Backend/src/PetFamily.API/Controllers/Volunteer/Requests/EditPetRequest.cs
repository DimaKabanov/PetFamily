using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.Commands.EditPet;
using PetFamily.Domain.Enums;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record EditPetRequest(
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
    public EditPetCommand ToCommand(Guid volunteerId, Guid petId) => 
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
    