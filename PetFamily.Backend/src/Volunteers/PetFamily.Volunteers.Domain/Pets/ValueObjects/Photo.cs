using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Domain.Pets.ValueObjects;

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