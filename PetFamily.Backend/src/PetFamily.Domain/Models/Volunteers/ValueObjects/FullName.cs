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
    
    public static FullName Create(string name, string surname, string? patronymic)
    {
        return new FullName(name, surname, patronymic);
    }
}