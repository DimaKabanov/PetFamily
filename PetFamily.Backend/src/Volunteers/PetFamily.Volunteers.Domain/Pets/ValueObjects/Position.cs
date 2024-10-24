using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pets.ValueObjects;

public record Position
{
    public static readonly Position First = new(1);
    
    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> Forward() => Create(Value + 1);
    
    public Result<Position, Error> Back() => Create(Value - 1);

    public static Result<Position, Error> Create(int position)
    {
        if (position < 1)
            return Errors.General.ValueIsInvalid("Position");
        
        return new Position(position);
    }
}