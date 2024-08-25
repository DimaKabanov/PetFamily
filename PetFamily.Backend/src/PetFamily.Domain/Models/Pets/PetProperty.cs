using PetFamily.Domain.Models.Pets.Ids;

namespace PetFamily.Domain.Models.Pets;

public record PetProperty
{
    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }
}