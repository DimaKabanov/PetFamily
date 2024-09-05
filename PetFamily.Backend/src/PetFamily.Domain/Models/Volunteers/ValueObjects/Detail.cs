using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Detail
{
    private Detail()
    {
    }
    
    public Detail(
        IEnumerable<SocialNetwork> socialNetworks,
        IEnumerable<Requisite> requisites)
    {
        SocialNetworks = socialNetworks.ToList();
        Requisites = requisites.ToList();
    }
    
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; } = [];
    
    public IReadOnlyList<Requisite> Requisites { get; } = [];
}