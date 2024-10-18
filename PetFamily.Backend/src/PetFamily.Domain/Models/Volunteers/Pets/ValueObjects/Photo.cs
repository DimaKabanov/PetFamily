namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record Photo
{
    public Photo(PhotoPath path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public PhotoPath Path { get; }

    public bool IsMain { get; }
}