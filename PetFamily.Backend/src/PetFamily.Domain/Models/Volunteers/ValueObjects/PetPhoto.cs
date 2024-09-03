namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record PetPhoto
{
    public string Path { get; }

    public bool IsMain { get; }
}