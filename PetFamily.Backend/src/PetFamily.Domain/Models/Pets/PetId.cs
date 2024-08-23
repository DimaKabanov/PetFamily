namespace PetFamily.Domain.Models.Pets;

public record PetId
{
    private PetId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static PetId NewId => new(Guid.NewGuid());

    public static PetId EmptyId => new(Guid.Empty);
}