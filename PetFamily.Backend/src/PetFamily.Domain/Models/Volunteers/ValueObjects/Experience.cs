namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Experience
{
    private Experience(int value)
    {
        Value = value;
    }
    
    public int Value { get; }
    
    public static Experience Create(int experience)
    {
        return new Experience(experience);
    }
}