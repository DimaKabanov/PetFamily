namespace PetFamily.SharedKernel.ValueObjects.EntityIds;

public record SpeciesId
{
    private SpeciesId(Guid id)
    {
        Value = id;
    }
    
    public Guid Value { get; }

    public static SpeciesId NewId() => new(Guid.NewGuid());

    public static SpeciesId EmptyId() => new(Guid.Empty);
    
    public static SpeciesId Create(Guid id) => new(id);
}