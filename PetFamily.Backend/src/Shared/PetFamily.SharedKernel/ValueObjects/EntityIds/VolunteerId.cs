namespace PetFamily.SharedKernel.ValueObjects.EntityIds;

public record VolunteerId
{
    private VolunteerId(Guid id)
    {
        Value = id;
    }
    
    public Guid Value { get; }

    public static VolunteerId NewId() => new(Guid.NewGuid());

    public static VolunteerId EmptyId() => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);
}