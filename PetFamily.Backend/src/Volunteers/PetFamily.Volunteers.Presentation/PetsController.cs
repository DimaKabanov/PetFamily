using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Queries.Pet.GetPet;
using PetFamily.Volunteers.Application.Queries.Pet.GetPets;
using PetFamily.Volunteers.Presentation.Pets.Requests;

namespace PetFamily.Volunteers.Presentation;

public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetPetsRequest request,
        [FromServices] GetPetsService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToQuery(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("{petId:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid petId,
        [FromServices] GetPetService service,
        CancellationToken ct)
    {
        var query = new GetPetQuery(petId);
        var result = await service.Handle(query, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}