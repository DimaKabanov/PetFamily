using PetFamily.Application.Volunteers.Commands.SetPetMainPhoto;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record SetPetMainPhotoRequest(string PhotoPath)
{
    public SetPetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new(volunteerId, petId, PhotoPath);
};