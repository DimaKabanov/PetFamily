using PetFamily.Domain.Models.Pets.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Pets;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }
    
    public string Name { get; private set; } = default!;
}