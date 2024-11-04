using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Species.Application.Commands.Breed.AddToSpecies;
using PetFamily.Species.Application.Commands.Breed.DeleteBreed;
using PetFamily.Species.Application.Commands.Species.Create;
using PetFamily.Species.Application.Commands.Species.DeleteSpecies;
using PetFamily.Species.Application.Queries.Breed.GetBreedsBySpecies;
using PetFamily.Species.Application.Queries.Species.GetSpecies;
using PetFamily.Species.Presentation.Requests;

namespace PetFamily.Species.Presentation;

public class SpeciesController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetSpeciesListRequest request,
        [FromServices] GetSpeciesService service,
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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateSpeciesRequest request,
        [FromServices] CreateService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPost("{id:guid}/breeds")]
    public async Task<ActionResult> AddBreed(
        [FromRoute] Guid id,
        [FromBody] AddBreedToSpeciesRequest request,
        [FromServices] AddToSpeciesService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
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

    [Authorize]
    [HttpDelete("{id:guid}/breeds/{breedId:guid}")]
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