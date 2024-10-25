using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record Description
{
    private Description(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Description, Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired("Description");

        if (description.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_HIGH_TEXT_LENGTH, "Description");
        
        return new Description(description);
    }
}