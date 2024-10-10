using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Species.Commands.DeleteSpecies;

namespace PetFamily.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteSpeciesById(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var command = new DeleteSpeciesCommand(id);
    }

    [HttpDelete("{id:guid}/species/{breedId:guid}")]
    public async Task<ActionResult<Guid>> DeleteBreedById(
        [FromRoute] Guid id,
        [FromRoute] Guid breedId,
        CancellationToken ct)
    {
        
    }
}l