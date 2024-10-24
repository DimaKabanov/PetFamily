using PetFamily.Volunteers.Application.Commands.Pet.UpdateStatus;
using PetFamily.Volunteers.Domain.Enums;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record UpdatePetStatusRequest(AssistanceStatus AssistanceStatus)
{
    public UpdateStatusCommand ToCommand(Guid volunteerId, Guid petId) => new(volunteerId, petId, AssistanceStatus);
};