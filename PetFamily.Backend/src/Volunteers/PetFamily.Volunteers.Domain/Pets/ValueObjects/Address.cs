using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pets.ValueObjects;

public record Address
{
    private Address(string street, int home, int flat)
    {
        Street = street;
        Home = home;
        Flat = flat;
    }

    public string Street { get; }

    public int Home { get; }

    public int Flat { get; }
    
    public static Result<Address, Error> Create(
        string street,
        int home,
        int flat)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsRequired("Street");

        if (street.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_MIDDLE_TEXT_LENGTH, "Street");

        if (home <= 0)
            return Errors.General.ValueIsInvalid("Home");
        
        if (flat <= 0)
            return Errors.General.ValueIsInvalid("Flat");
        
        return new Address(street, home, flat);
    }
}