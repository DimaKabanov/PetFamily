using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record Experience
{
    public const int MIN_EXPERIENCE = 0;
    public const int MAX_EXPERIENCE = 99;
    
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