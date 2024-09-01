using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Experience
{
    private const int MIN_EXPERIENCE = 0;
    private const int MAX_EXPERIENCE = 99;
    
    private Experience(int value)
    {
        Value = value;
    }
    
    public int Value { get; }
    
    public static Result<Experience, Error> Create(int experience)
    {
        if (experience is < MIN_EXPERIENCE or > MAX_EXPERIENCE)
            return Errors.General.ValueIsInvalid("Experience");
        
        return new Experience(experience);
    }
}