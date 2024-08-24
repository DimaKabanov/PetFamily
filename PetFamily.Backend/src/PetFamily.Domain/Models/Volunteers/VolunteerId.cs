namespace PetFamily.Domain.Models.Volunteers;

public record VolunteerId
{
    private VolunteerId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static VolunteerId NewId => new(Guid.NewGuid());

    public static VolunteerId EmptyId => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);
}