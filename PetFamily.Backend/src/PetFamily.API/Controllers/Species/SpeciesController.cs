using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.Commands.DeleteBreed;
using PetFamily.Application.Species.Commands.DeleteSpecies;

namespace PetFamily.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteSpeciesService service,
        CancellationToken ct)
    {
        var command = new DeleteSpeciesCommand(id);
        var result = await service.Handle(command, ct);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/breed/{breedId:guid}")]
    public async Task<ActionResult<Guid>> DeleteBreed(
        [FromRoute] Guid id,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedService service,
        CancellationToken ct)
    {
        var command = new DeleteBreedCommand(id, breedId);
        var result = await service.Handle(command, ct);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}