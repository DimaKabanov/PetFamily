using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record SerialNumber
{
    private SerialNumber(int value)
    {
        Value = value;
    }

    public int Value { get; }
    
    public static Result<SerialNumber, Error> Create(int serialNumber)
    {
        if (serialNumber <= 0)
            return Errors.General.ValueIsInvalid("SerialNumber");
        
        return new SerialNumber(serialNumber);
    }
}