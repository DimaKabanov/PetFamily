using PetFamily.Domain.Models.Species;

namespace PetFamily.Domain.Models.Pets.ValueObjects;

public record PetProperty
{
    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }
}