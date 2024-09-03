using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Requisite
{
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public static Result<Requisite, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired("Name");

        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Name");
        
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired("Description");

        if (description.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Description");
        
        return new Requisite(name, description);
    }
}