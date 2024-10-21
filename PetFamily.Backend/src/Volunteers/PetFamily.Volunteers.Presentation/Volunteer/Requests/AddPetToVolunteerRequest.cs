using PetFamily.Core.Dto;
using PetFamily.Volunteers.Application.Commands.Pet.AddToVolunteer;
using PetFamily.Volunteers.Domain.Enums;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record AddPetToVolunteerRequest(
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
    public AddToVolunteerCommand ToCommand(Guid volunteerId) => 
        new(
            volunteerId,
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
    