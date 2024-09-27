namespace PetFamily.Domain.Models.Volunteers.Pets;

public record PetId
{
    private PetId(Guid id)
    {
        Value = id;
    }
    
    public Guid Value { get; }

    public static PetId NewId() => new(Guid.NewGuid());

    public static PetId EmptyId() => new(Guid.Empty);
    
    public static PetId Create(Guid id) => new(id);
}