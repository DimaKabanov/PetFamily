using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record PhysicalProperty
{
    public const double MIN_WEIGHT = 0.1;
    public const double MAX_WEIGHT = 50;
    public const double MIN_HEIGHT = 0.1;
    public const double MAX_HEIGHT = 15;
    
    private PhysicalProperty(string color, string health, double weight, double height)
    {
        Color = color;
        Health = health;
        Weight = weight;
        Height = height;
    }

    public string Color { get; } = default!;
    
    public string Health { get; } = default!;
    
    public double Weight { get; }
    
    public double Height { get; }
    
    public static Result<PhysicalProperty, Error> Create(
        string color,
        string health,
        double weight,
        double height)
    {
        if (string.IsNullOrWhiteSpace(color))
            return Errors.General.ValueIsRequired("Color");

        if (color.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Color");
        
        if (string.IsNullOrWhiteSpace(health))
            return Errors.General.ValueIsRequired("Health");

        if (health.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Health");

        if (weight is < MIN_WEIGHT or > MAX_WEIGHT)
            return Errors.General.ValueIsInvalid("Weight");
        
        if (height is < MIN_HEIGHT or > MAX_HEIGHT)
            return Errors.General.ValueIsInvalid("Height");
        
        return new PhysicalProperty(color, health, weight, height);
    }
}