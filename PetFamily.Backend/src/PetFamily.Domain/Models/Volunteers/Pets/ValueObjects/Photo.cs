using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record Photo
{
    private Photo(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; }

    public bool IsMain { get; }
    
    public static Result<Photo, Error> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsRequired("Path");

        if (path.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_MIDDLE_TEXT_LENGTH, "Path");
        
        return new Photo(path, isMain);
    }
}