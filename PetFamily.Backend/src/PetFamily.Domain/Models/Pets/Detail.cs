namespace PetFamily.Domain.Models.Pets;

public class Detail
{
    public List<PetPhoto> Photos { get; private set; } = [];
    
    public List<Requisite> Requisites { get; private set; } = [];
}