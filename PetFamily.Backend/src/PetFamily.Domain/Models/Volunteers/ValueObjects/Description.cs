namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Description
{
    private Description(string description)
    {
        Value = description;
    }
    
    public string Value { get; }
    
    public static Description Create(string description)
    {
        return new Description(description);
    }
}