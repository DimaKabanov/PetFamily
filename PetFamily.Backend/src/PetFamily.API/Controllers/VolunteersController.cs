using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers;

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
    public async Task<ActionResult> Update(
        [FromServices] UpdateVolunteerMainInfoService service,
        [FromServices] IValidator<UpdateVolunteerMainInfoRequest> validator,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerMainInfoRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await service.Update(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}