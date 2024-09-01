namespace PetFamily.Domain.Models.Pets.ValueObjects;

public record Detail
{
    public List<PetPhoto> Photos { get; } = [];
    
    public List<Requisite> Requisites { get; } = [];
}