namespace PetFamily.Domain.Models;

public record Requisite
{
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public static Requisite Create(string name, string description)
    {
        return new Requisite(name, description);
    }
}