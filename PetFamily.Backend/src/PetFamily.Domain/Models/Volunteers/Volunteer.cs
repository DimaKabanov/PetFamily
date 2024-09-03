using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Ids;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private Volunteer(VolunteerId id) : base(id)
    {
    }
    
    public Volunteer(
        VolunteerId id,
        VolunteerFullName volunteerFullName,
        VolunteerDescription volunteerDescription,
        VolunteerExperience volunteerExperience,
        VolunteerPhone volunteerPhone,
        VolunteerDetail volunteerDetails
    ) : base(id)
    {
        VolunteerFullName = volunteerFullName;
        VolunteerDescription = volunteerDescription;
        VolunteerExperience = volunteerExperience;
        VolunteerPhone = volunteerPhone;
        VolunteerDetails = volunteerDetails;
    }
    
    public VolunteerFullName VolunteerFullName { get; private set; }

    public VolunteerDescription VolunteerDescription { get; private set; }

    public VolunteerExperience VolunteerExperience { get; private set; }
    
    public VolunteerPhone VolunteerPhone { get; private set; }

    public VolunteerDetail VolunteerDetails { get; private set; }

    public List<Pet> Pets { get; private set; } = [];
    
    public int PetsNeedsHelpCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
    
    public int PetsSearchHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.SearchAHome);
    
    public int PetsFoundHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundAHome);
}