using PetFamily.Application.Species.Commands.CreateSpecies;

namespace PetFamily.API.Controllers.Species.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() => new(Name);
};