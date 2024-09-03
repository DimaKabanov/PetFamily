using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record VolunteerPhone
{
    private const int PHONE_LENGTH = 11;
    
    private VolunteerPhone(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<VolunteerPhone, Error> Create(string phone)
    {
        if (phone.Length != PHONE_LENGTH)
            return Errors.General.ValueIsInvalid("Phone");
        
        return new VolunteerPhone(phone);
    }
}