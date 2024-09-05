using PetFamily.Domain.Models.Species.Breeds;
using PetFamily.Domain.Models.Species.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Species;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public Species(SpeciesId id, Name name,  List<Breed> breeds) : base(id)
    {
        Name = name;
        _breeds = breeds;
    }
    
    public Name Name { get; private set; } = default!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;
}