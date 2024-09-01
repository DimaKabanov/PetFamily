namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Phone
{
    private Phone(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Phone Create(string phone)
    {
        return new Phone(phone);
    }
}