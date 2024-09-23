using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record Photo
{
    public Photo(FilePath path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public FilePath Path { get; }

    public bool IsMain { get; }
}