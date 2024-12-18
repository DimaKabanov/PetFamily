namespace PetFamily.SharedKernel.ValueObjects.EntityIds;

public record BreedId
{
    private BreedId(Guid id)
    {
        Value = id;
    }
    
    public Guid Value { get; }

    public static BreedId NewId() => new(Guid.NewGuid());

    public static BreedId EmptyId() => new(Guid.Empty);
    
    public static BreedId Create(Guid id) => new(id);
}