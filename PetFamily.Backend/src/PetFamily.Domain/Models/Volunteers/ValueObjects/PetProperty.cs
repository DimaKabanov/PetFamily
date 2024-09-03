using PetFamily.Domain.Models.Species.Ids;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record PetProperty
{
    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }
}