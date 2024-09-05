namespace PetFamily.Domain.Models.Species;

public record SpeciesId
{
    private SpeciesId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static SpeciesId NewId => new(Guid.NewGuid());

    public static SpeciesId EmptyId => new(Guid.Empty);
    
    public static SpeciesId Create(Guid id) => new(id);
}