namespace PetFamily.Domain.Models.Volunteers;

public record Detail
{
    public List<SocialNetwork> SocialNetworks { get; private set; } = [];
    
    public List<Requisite> Requisites { get; private set; } = [];
}