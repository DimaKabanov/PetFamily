using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Species.Breeds;
using PetFamily.Domain.Models.Species.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Species;

public class Species : Shared.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public Species(SpeciesId id, Name name) : base(id)
    {
        Name = name;
    }
    
    public Name Name { get; private set; } = default!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;
    
    public Result<Breed, Error> GetBreedById(BreedId breedId)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == breedId);
        if (breed is null)
            return Errors.General.NotFound(breedId.Value);

        return breed;
    }
    
    public Result<Breed, Error> GetBreedByName(Breeds.ValueObjects.Name name)
    {
        var breed = _breeds.FirstOrDefault(b => b.Name == name);
        if (breed is null)
            return Errors.General.NotFound();

        return breed;
    }

    public void AddBreed(Breed breed) => _breeds.Add(breed);

    public void DeleteBreed(Breed breed) => _breeds.Remove(breed);
}