using PetFamily.Domain.Models.Pets.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Pets;

public class Species : Entity<SpeciesId>
{
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public string Name { get; private set; } = default!;
    
    public List<Breed> Breeds { get; private set; } = [];
}