using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisiteDto> Requisites) : ICommand;