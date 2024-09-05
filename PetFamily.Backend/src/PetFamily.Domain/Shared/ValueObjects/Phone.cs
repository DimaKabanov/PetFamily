using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Phone
{
    public const int PHONE_LENGTH = 11;
    
    private Phone(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Phone, Error> Create(string phone)
    {
        if (phone.Length != PHONE_LENGTH)
            return Errors.General.ValueIsInvalid("Phone");
        
        return new Phone(phone);
    }
}