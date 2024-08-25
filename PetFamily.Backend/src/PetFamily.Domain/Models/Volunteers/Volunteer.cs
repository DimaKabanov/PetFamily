using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Pets;
using PetFamily.Domain.Models.Volunteers.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private Volunteer(VolunteerId id) : base(id)
    {
    }
    public string FullName { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public int Experience { get; private set; }
    
    public string Phone { get; private set; } = default!;

    public Detail Details { get; private set; }

    public List<Pet> Pets { get; private set; } = [];
    
    public int PetsNeedsHelpCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
    
    public int PetsSearchHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.SearchAHome);
    
    public int PetsFoundHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundAHome);
}