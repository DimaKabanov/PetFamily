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
        var result = await service.Handle(request.ToQuery(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<ActionResult> GetBreedsBySpecies(
        [FromRoute] Guid speciesId,
        [FromServices] GetBreedsBySpeciesService service,
        CancellationToken ct)
    {
        var query = new GetBreedsBySpeciesQuery(speciesId);
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
    [HttpPost("{speciesId:guid}/breeds")]
    public async Task<ActionResult> AddBreed(
        [FromRoute] Guid speciesId,
        [FromBody] AddBreedToSpeciesRequest request,
        [FromServices] AddToSpeciesService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(speciesId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{speciesId:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid speciesId,
        [FromServices] DeleteSpeciesService service,
        CancellationToken ct)
    {
        var command = new DeleteSpeciesCommand(speciesId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    public async Task<ActionResult<Guid>> DeleteBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedService service,
        CancellationToken ct)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}