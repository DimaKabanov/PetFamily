using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private Volunteer(VolunteerId id) : base(id)
    {
    }
    
    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description description,
        Experience experience,
        Phone phone,
        Detail details) : base(id)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        Phone = phone;
        Details = details;
    }
    
    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public Experience Experience { get; private set; }
    
    public Phone Phone { get; private set; }

    public Detail Details { get; private set; }

    public List<Pet> Pets { get; private set; } = [];
    
    public int PetsNeedsHelpCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
    
    public int PetsSearchHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.SearchAHome);
    
    public int PetsFoundHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundAHome);

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Experience experience,
        Phone phone)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        Phone = phone;
    }
}