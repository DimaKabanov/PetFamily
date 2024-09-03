using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record VolunteerDetail
{
    private VolunteerDetail(
        IEnumerable<VolunteerSocialNetwork> socialNetworks,
        IEnumerable<Requisite> requisites)
    {
        SocialNetworks = socialNetworks.ToList();
        Requisites = requisites.ToList();
    }
    
    public IReadOnlyList<VolunteerSocialNetwork> SocialNetworks { get; } = [];
    
    public IReadOnlyList<Requisite> Requisites { get; } = [];
    
    public static VolunteerDetail Create(
        IEnumerable<VolunteerSocialNetwork> socialNetworks,
        IEnumerable<Requisite> requisites)
    {
        return new VolunteerDetail(socialNetworks, requisites);
    }
}