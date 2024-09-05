using PetFamily.Domain.Models.Species;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record Property
{
    public Property(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }
}