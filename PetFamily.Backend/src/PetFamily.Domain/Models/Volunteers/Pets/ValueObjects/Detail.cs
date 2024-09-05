using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

public record Detail
{
    private Detail()
    {
    }
    
    public Detail(
        IEnumerable<Photo> photos,
        IEnumerable<Requisite> requisites)
    {
        Photos = photos.ToList();
        Requisites = requisites.ToList();
    }

    public IReadOnlyList<Photo> Photos { get; } = [];
    
    public IReadOnlyList<Requisite> Requisites { get; } = [];
}