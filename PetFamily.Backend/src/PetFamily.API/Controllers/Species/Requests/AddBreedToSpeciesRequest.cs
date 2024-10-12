using PetFamily.Application.Species.Commands.AddBreedToSpecies;
using PetFamily.Domain.Models.Species;

namespace PetFamily.API.Controllers.Species.Requests;

public record AddBreedToSpeciesRequest(string Name)
{
    public AddBreedToSpeciesCommand ToCommand(Guid speciesId) => new(speciesId, Name);
};