using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.Commands.DeleteBreed;
using PetFamily.Application.Species.Commands.DeleteSpecies;
using PetFamily.Application.Species.Queries.GetBreedsBySpecies;
using PetFamily.Application.Species.Queries.GetSpeciesList;

namespace PetFamily.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetSpecies(
        [FromQuery] GetSpeciesListRequest request,
        [FromServices] GetSpeciesListService service,
        CancellationToken ct)
    {
        var response = await service.Handle(request.ToQuery(), ct);

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/breeds")]
    public async Task<ActionResult> GetBreedsBySpecies(
        [FromRoute] Guid id,
        [FromServices] GetBreedsBySpeciesService service,
        CancellationToken ct)
    {
        var query = new GetBreedsBySpeciesQuery(id);
        var result = await service.Handle(query, ct);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
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