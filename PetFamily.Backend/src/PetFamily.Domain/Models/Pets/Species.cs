using PetFamily.Domain.Models.Pets.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Pets;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    private Species(SpeciesId id, string name,  List<Breed> breeds) : base(id)
    {
        Name = name;
        _breeds = breeds;
    }
    
    public string Name { get; private set; } = default!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;
    
    public static Species Create(SpeciesId id, string name, List<Breed> breeds)
    {
        var species = new Species(id, name, breeds);

        return species;
    }
}