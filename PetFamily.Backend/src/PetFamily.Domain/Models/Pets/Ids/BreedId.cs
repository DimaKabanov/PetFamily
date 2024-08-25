namespace PetFamily.Domain.Models.Pets.Ids;

public record BreedId
{
    private BreedId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static BreedId NewId => new(Guid.NewGuid());

    public static BreedId EmptyId => new(Guid.Empty);
    
    public static BreedId Create(Guid id) => new(id);
}