using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Domain.Breeds;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Domain;

public class Species : SharedKernel.Entity<SpeciesId>
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

    public UnitResult<Error> DeleteBreed(BreedId breedId)
    {
        var breedResult = GetBreedById(breedId);
        if (breedResult.IsFailure)
            return breedResult.Error;
        
        _breeds.Remove(breedResult.Value);

        return Result.Success<Error>();
    }
}