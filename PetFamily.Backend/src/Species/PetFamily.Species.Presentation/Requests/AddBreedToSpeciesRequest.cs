using PetFamily.Species.Application.Commands.Breed.AddToSpecies;

namespace PetFamily.Species.Presentation.Requests;

public record AddBreedToSpeciesRequest(string Name)
{
    public AddToSpeciesCommand ToCommand(Guid speciesId) => new(speciesId, Name);
};