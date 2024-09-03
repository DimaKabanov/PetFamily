using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record VolunteerDescription
{
    private VolunteerDescription(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<VolunteerDescription, Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired("Description");

        if (description.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_HIGH_TEXT_LENGTH, "Description");
        
        return new VolunteerDescription(description);
    }
}