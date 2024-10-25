using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pets.ValueObjects;

public record CreatedDate
{
    private CreatedDate(DateTime value)
    {
        Value = value;
    }
    
    public DateTime Value { get; }
    
    public static Result<CreatedDate, Error> Create(DateTime createdDate)
    {
        if (createdDate > DateTime.Now)
            return Errors.General.ValueIsInvalid("CreatedDate");
        
        return new CreatedDate(createdDate);
    }
}