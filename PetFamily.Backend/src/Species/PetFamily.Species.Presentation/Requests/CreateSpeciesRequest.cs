using PetFamily.Species.Application.Commands.Species.Create;

namespace PetFamily.Species.Presentation.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateCommand ToCommand() => new(Name);
};