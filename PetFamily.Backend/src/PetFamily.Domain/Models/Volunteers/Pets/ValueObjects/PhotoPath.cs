using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record PhotoPath
{
    private PhotoPath(string path)
    {
        Path = path;
    }

    public string Path { get; }
    
    public static Result<PhotoPath, Error> Create(Guid path, string extension)
    {
        var fullPath = $"{path}.{extension}";
        
        return new PhotoPath(fullPath);
    }
    
    public static Result<PhotoPath, Error> Create(string fullPath)
    {
        return new PhotoPath(fullPath);
    }
}