namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Detail
{
    private Detail(IEnumerable<SocialNetwork> socialNetworks, IEnumerable<Requisite> requisites)
    {
        SocialNetworks = socialNetworks.ToList();
        Requisites = requisites.ToList();
    }
    
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; } = [];
    
    public IReadOnlyList<Requisite> Requisites { get; } = [];
    
    public static Detail Create(IEnumerable<SocialNetwork> socialNetworks, IEnumerable<Requisite> requisites)
    {
        return new Detail(socialNetworks, requisites);
    }
}