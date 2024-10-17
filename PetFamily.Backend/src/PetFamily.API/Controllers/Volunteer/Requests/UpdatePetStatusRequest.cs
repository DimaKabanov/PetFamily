using PetFamily.Application.Volunteers.Commands.UpdatePetStatus;
using PetFamily.Domain.Enums;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record UpdatePetStatusRequest(AssistanceStatus AssistanceStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId) => new(volunteerId, petId, AssistanceStatus);
};