namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string title, string url)
    {
        Title = title;
        Url = url;
    }
    
    public string Title { get; }

    public string Url { get; }
    
    public static SocialNetwork Create(string title, string url)
    {
        return new SocialNetwork(title, url);
    }
}