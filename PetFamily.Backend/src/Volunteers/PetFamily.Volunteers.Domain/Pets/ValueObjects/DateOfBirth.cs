using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pets.ValueObjects;

public record DateOfBirth
{
    private DateOfBirth(DateOnly value)
    {
        Value = value;
    }
    
    public DateOnly Value { get; }
    
    public static Result<DateOfBirth, Error> Create(DateOnly dateOfBirth)
    {
        if (dateOfBirth > DateOnly.FromDateTime(DateTime.Now))
            return Errors.General.ValueIsInvalid("DateOfBirth");
        
        return new DateOfBirth(dateOfBirth);
    }
}