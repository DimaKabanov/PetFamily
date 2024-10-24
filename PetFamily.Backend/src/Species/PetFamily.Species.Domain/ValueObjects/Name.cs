using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Domain.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }
    
    public static Result<Name, Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired("Name");

        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Name");
        
        return new Name(name);
    }
}