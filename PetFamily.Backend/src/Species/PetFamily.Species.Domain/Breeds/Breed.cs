using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Domain.Breeds.ValueObjects;

namespace PetFamily.Species.Domain.Breeds;

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