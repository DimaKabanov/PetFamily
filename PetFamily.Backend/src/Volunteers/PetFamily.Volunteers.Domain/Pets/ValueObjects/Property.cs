using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Volunteers.Domain.Pets.ValueObjects;

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