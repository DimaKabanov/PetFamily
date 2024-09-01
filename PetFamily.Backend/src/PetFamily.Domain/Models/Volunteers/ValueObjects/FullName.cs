using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record FullName
{
    private FullName(string name, string surname, string? patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }
    
    public string Name { get; }
    
    public string Surname { get; }

    public string? Patronymic { get; }
    
    public static Result<FullName, Error> Create(string name, string surname, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired("Name");

        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Name");
        
        if (string.IsNullOrWhiteSpace(surname))
            return Errors.General.ValueIsRequired("Surname");

        if (surname.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Surname");
        
        if (patronymic is not null && patronymic.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Patronymic");
        
        return new FullName(name, surname, patronymic);
    }
}