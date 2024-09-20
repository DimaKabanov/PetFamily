using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {
        RuleFor(u => u.BucketName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.Stream).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}