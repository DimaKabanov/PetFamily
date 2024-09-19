using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Files.Upload;

namespace PetFamily.API.Controllers;

public class FilesController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromServices] UploadFileService service,
        [FromServices] IValidator<UploadFileRequest> validator,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        const string BUCKET_NAME = "files";
        var fileName = Guid.NewGuid().ToString();
        
        var request = new UploadFileRequest(stream, BUCKET_NAME, fileName);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await service.Upload(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
}