using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

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