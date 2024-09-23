using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteer;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerService service,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Create(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateVolunteerMainInfoService service,
        [FromServices] IValidator<UpdateVolunteerMainInfoRequest> validator,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerMainInfoRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await service.Update(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromServices] UpdateVolunteerSocialNetworksService service,
        [FromServices] IValidator<UpdateVolunteerSocialNetworksRequest> validator,
        [FromBody] UpdateVolunteerSocialNetworksDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerSocialNetworksRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await service.Update(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid id,
        [FromServices] UpdateVolunteerRequisitesService service,
        [FromServices] IValidator<UpdateVolunteerRequisitesRequest> validator,
        [FromBody] UpdateVolunteerRequisitesDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerRequisitesRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await service.Update(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerService service,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await service.Delete(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromForm] AddPetRequest request,
        [FromServices] AddPetService service,
        CancellationToken cancellationToken)
    {
        await using var photoProcessor = new FormPhotoProcessor();

        var photoDtos = photoProcessor.Process(request.Photos);

        var command = new AddPetCommand(
            id,
            request.Name,
            request.Description,
            request.PhysicalProperty,
            request.Address,
            request.Phone,
            request.IsCastrated,
            request.DateOfBirth,
            request.IsVaccinated,
            request.AssistanceStatus,
            request.CreatedDate,
            request.Requisites,
            photoDtos);
        
        var result = await service.AddPet(command, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}