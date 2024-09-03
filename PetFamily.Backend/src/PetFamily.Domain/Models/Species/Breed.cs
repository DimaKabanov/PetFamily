using PetFamily.Domain.Models.Species.Ids;
using PetFamily.Domain.Models.Species.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Species;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }
    
    public BreedName BreedName { get; private set; } = default!;
}