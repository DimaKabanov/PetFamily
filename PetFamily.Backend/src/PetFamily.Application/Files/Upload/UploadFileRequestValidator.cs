using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {
        RuleFor(u => u.bucketName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.fileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.stream).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}