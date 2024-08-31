using PetFamily.Domain.Models.Breed.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Breed;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }
    
    public Name Name { get; private set; } = default!;
}