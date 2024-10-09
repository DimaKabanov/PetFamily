using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.Commands.AddPetToVolunteer;
using PetFamily.Domain.Enums;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record AddPetToVolunteerRequest(
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
    public AddPetToVolunteerCommand ToCommand(Guid volunteerId) => 
        new(
            volunteerId, 
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
    