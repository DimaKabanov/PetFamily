using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record Position
{
    public static Position First = new(1);
    
    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }
    
    public static Result<Position, Error> Create(int position)
    {
        if (position <= 0)
            return Errors.General.ValueIsInvalid("Position");
        
        return new Position(position);
    }
}