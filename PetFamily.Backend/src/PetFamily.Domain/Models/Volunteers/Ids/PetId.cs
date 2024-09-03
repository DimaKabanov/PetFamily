namespace PetFamily.Domain.Models.Volunteers.Ids;

public record PetId
{
    private PetId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static PetId NewId => new(Guid.NewGuid());

    public static PetId EmptyId => new(Guid.Empty);
    
    public static PetId Create(Guid id) => new(id);
}