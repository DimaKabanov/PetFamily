using PetFamily.Volunteers.Application.Commands.Pet.SetMainPhoto;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record SetPetMainPhotoRequest(string PhotoPath)
{
    public SetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId) => new(volunteerId, petId, PhotoPath);
};