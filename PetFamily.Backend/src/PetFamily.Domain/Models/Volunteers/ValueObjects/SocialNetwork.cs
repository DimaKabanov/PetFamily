using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

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
    
    public static Result<SocialNetwork, Error> Create(string title, string url)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsRequired("Title");

        if (title.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Title");
        
        if (string.IsNullOrWhiteSpace(url))
            return Errors.General.ValueIsRequired("Url");

        if (url.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueTooLong(Constants.MAX_LOW_TEXT_LENGTH, "Url");
        
        return new SocialNetwork(title, url);
    }
}