using PetFamily.Domain.Models.Species.Breeds.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Species.Breeds;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    public Breed(BreedId id, Name name) : base(id)
    {
        Name = name;
    }

    public Name Name { get; private set; }
}