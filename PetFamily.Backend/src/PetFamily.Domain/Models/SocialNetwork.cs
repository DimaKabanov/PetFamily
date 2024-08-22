namespace PetFamily.Domain.Models;

public class SocialNetwork
{
    public Guid Id { get; private set; }
    
    public string Title { get; private set; } = default!;

    public string Url { get; private set; } = default!;
}