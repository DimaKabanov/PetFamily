namespace PetFamily.SharedKernel;

public abstract class Entity<TId>
{
    protected Entity(TId id)
    {
        Id = id;
    }
    
    public TId Id { get; private set; }
}