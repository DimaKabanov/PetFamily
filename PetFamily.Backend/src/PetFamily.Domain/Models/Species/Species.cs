using PetFamily.Domain.Models.Species.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Species;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed.Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    private Species(SpeciesId id, Name name,  List<Breed.Breed> breeds) : base(id)
    {
        Name = name;
        _breeds = breeds;
    }
    
    public Name Name { get; private set; } = default!;
    
    public IReadOnlyList<Breed.Breed> Breeds => _breeds;
    
    public static Species Create(SpeciesId id, Name name, List<Breed.Breed> breeds)
    {
        var species = new Species(id, name, breeds);

        return species;
    }
}