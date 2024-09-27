namespace PetFamily.Application.Volunteers.AddPetToVolunteer;

public record AddPetToVolunteerRequest(Guid VolunteerId, AddPetToVolunteerDto Dto);
    