namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record Detail
{
    private Detail(List<SocialNetwork> socialNetworks, List<Requisite> requisites)
    {
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }
    
    public List<SocialNetwork> SocialNetworks { get; } = [];
    
    public List<Requisite> Requisites { get; } = [];
    
    public static Detail Create(List<SocialNetwork> socialNetworks, List<Requisite> requisites)
    {
        return new Detail(socialNetworks, requisites);
    }
}