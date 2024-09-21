namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record PhotoList
{
    private PhotoList()
    {
    }
    
    public PhotoList(IEnumerable<Photo> photos)
    {
        Photos = photos.ToList();
    }

    public IReadOnlyList<Photo> Photos { get; }
}