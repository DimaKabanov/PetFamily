using PetFamily.Domain.Enums;

namespace PetFamily.Domain.Models;

public class Volunteer
{
    public Guid Id { get; private set; }

    public string FullName { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public int Experience { get; private set; }
    
    public string Phone { get; private set; } = default!;
    
    public List<SocialNetwork> SocialNetworks { get; private set; } = [];
    
    public List<Requisite> Requisites { get; private set; } = [];

    public List<Pet> Pets { get; private set; } = [];
    
    public int PetsNeedsHelpCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
    
    public int PetsSearchHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.SearchAHome);
    
    public int PetsFoundHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundAHome);
}