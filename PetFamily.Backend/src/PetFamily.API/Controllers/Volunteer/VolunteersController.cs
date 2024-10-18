using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteer.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Volunteers.Commands.AddPetToVolunteer;
using PetFamily.Application.Volunteers.Commands.AddPhotoToPet;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.Delete;
using PetFamily.Application.Volunteers.Commands.DeletePhotosFromPet;
using PetFamily.Application.Volunteers.Commands.HardDeletePet;
using PetFamily.Application.Volunteers.Commands.SetPetMainPhoto;
using PetFamily.Application.Volunteers.Commands.SoftDeletePet;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Commands.UpdatePet;
using PetFamily.Application.Volunteers.Commands.UpdatePetStatus;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;
using PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;
using PetFamily.Application.Volunteers.Queries.GetVolunteer;
using PetFamily.Application.Volunteers.Queries.GetVolunteers;

namespace PetFamily.API.Controllers.Volunteer;

public class VolunteersController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetVolunteersRequest request,
        [FromServices] GetVolunteersService service,
        CancellationToken ct)
    {
        var response = await service.Handle(request.ToQuery(), ct);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetVolunteerService service,
        CancellationToken ct)
    {
        var query = new GetVolunteerQuery(id);
        var result = await service.Handle(query, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerService service,
        CancellationToken ct)
    { 
        var result = await service.Handle(request.ToCommand(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoRequest request,
        [FromServices] UpdateVolunteerMainInfoService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworksRequest request,
        [FromServices] UpdateVolunteerSocialNetworksService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequisitesRequest request,
        [FromServices] UpdateVolunteerRequisitesService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerService service,
        CancellationToken ct)
    {
        var command = new DeleteVolunteerCommand(id);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetToVolunteerRequest request,
        [FromServices] AddPetToVolunteerService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/pet/{petId:guid}")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] UpdatePetService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id, petId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult<Guid>> AddPhotosToPet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromForm] AddPhotoToPetRequest request,
        [FromServices] UploadPhotoToPetService service,
        CancellationToken ct)
    {
        await using var photoProcessor = new FormPhotoProcessor();
        var photoDtos = photoProcessor.Process(request.Photos);
        var command = new UploadPhotoToPetCommand(id, petId, photoDtos);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/pet/{petId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdatePetStatus(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetStatusRequest request,
        [FromServices] UpdatePetStatusService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id, petId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/pet/{petId:guid}/main-photo")]
    public async Task<ActionResult<Guid>> SetPetMainPhoto(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] SetPetMainPhotoRequest request,
        [FromServices] SetPetMainPhotoService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(id, petId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult<Guid>> DeletePhotosFromPet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] DeletePhotosFromPetService service,
        CancellationToken ct)
    {
        var command = new DeletePhotosFromPetCommand(id, petId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult<Guid>> SoftDeletePet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] SoftDeletePetService service,
        CancellationToken ct)
    {
        var command = new SoftDeletePetCommand(id, petId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDeletePet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] HardDeletePetService service,
        CancellationToken ct)
    {
        var command = new HardDeletePetCommand(id, petId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}