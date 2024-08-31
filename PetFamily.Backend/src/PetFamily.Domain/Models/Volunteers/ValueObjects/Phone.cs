namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Phone
{
    private Phone(string phone)
    {
        Value = phone;
    }
    
    public string Value { get; }
    
    public static Phone Create(string phone)
    {
        return new Phone(phone);
    }
}