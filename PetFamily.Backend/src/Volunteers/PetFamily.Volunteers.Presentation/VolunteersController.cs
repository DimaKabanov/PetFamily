using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.Pet.AddToVolunteer;
using PetFamily.Volunteers.Application.Commands.Pet.DeletePhotos;
using PetFamily.Volunteers.Application.Commands.Pet.HardDelete;
using PetFamily.Volunteers.Application.Commands.Pet.SetMainPhoto;
using PetFamily.Volunteers.Application.Commands.Pet.SoftDelete;
using PetFamily.Volunteers.Application.Commands.Pet.UpdateMainInfo;
using PetFamily.Volunteers.Application.Commands.Pet.UpdateStatus;
using PetFamily.Volunteers.Application.Commands.Pet.UploadPhoto;
using PetFamily.Volunteers.Application.Commands.Volunteer.Create;
using PetFamily.Volunteers.Application.Commands.Volunteer.Delete;
using PetFamily.Volunteers.Application.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Volunteers.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Volunteers.Application.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteer;
using PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteers;
using PetFamily.Volunteers.Presentation.Processors;
using PetFamily.Volunteers.Presentation.Volunteer.Requests;

namespace PetFamily.Volunteers.Presentation;

public class VolunteersController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetVolunteersRequest request,
        [FromServices] GetVolunteersService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToQuery(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid volunteerId,
        [FromServices] GetVolunteerService service,
        CancellationToken ct)
    {
        var query = new GetVolunteerQuery(volunteerId);
        var result = await service.Handle(query, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerService service,
        CancellationToken ct)
    { 
        var result = await service.Handle(request.ToCommand(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{volunteerId:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerMainInfoRequest request,
        [FromServices] UpdateVolunteerMainInfoService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(volunteerId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{volunteerId:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerSocialNetworksRequest request,
        [FromServices] UpdateVolunteerSocialNetworksService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(volunteerId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{volunteerId:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerRequisitesRequest request,
        [FromServices] UpdateVolunteerRequisitesService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(volunteerId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{volunteerId:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid volunteerId,
        [FromServices] DeleteVolunteerService service,
        CancellationToken ct)
    {
        var command = new DeleteVolunteerCommand(volunteerId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetToVolunteerRequest request,
        [FromServices] AddToVolunteerService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(volunteerId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] UpdateMainInfoService mainInfoService,
        CancellationToken ct)
    {
        var result = await mainInfoService.Handle(request.ToCommand(volunteerId, petId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult<Guid>> AddPhotosToPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPhotoToPetRequest request,
        [FromServices] UploadPhotoService service,
        CancellationToken ct)
    {
        await using var photoProcessor = new FormPhotoProcessor();
        var photoDtos = photoProcessor.Process(request.Photos);
        var command = new UploadPhotoCommand(volunteerId, petId, photoDtos);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdatePetStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetStatusRequest request,
        [FromServices] UpdateStatusService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(volunteerId, petId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/main-photo")]
    public async Task<ActionResult<Guid>> SetPetMainPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] SetPetMainPhotoRequest request,
        [FromServices] SetMainPhotoService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(volunteerId, petId), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult<Guid>> DeletePhotosFromPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] DeletePhotosService service,
        CancellationToken ct)
    {
        var command = new DeletePhotosCommand(volunteerId, petId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult<Guid>> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] SoftDeleteService service,
        CancellationToken ct)
    {
        var command = new SoftDeleteCommand(volunteerId, petId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardDeleteService service,
        CancellationToken ct)
    {
        var command = new HardDeleteCommand(volunteerId, petId);
        var result = await service.Handle(command, ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}