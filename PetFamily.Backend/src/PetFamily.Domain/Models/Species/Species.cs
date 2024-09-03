using PetFamily.Domain.Models.Species.Ids;
using PetFamily.Domain.Models.Species.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Species;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    private Species(SpeciesId id, SpeciesName speciesName,  List<Breed> breeds) : base(id)
    {
        SpeciesName = speciesName;
        _breeds = breeds;
    }
    
    public SpeciesName SpeciesName { get; private set; } = default!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;
    
    public static Species Create(SpeciesId id, SpeciesName speciesName, List<Breed> breeds)
    {
        var species = new Species(id, speciesName, breeds);

        return species;
    }
}