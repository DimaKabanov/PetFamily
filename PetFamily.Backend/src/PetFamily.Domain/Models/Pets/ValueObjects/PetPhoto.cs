namespace PetFamily.Domain.Models.Pets.ValueObjects;

public record PetPhoto
{
    public string Path { get; }

    public bool IsMain { get; }
}