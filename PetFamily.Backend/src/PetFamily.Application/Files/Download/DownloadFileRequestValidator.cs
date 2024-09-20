using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Download;

public class DownloadFileRequestValidator : AbstractValidator<DownloadFileRequest>
{
    public DownloadFileRequestValidator()
    {
        RuleFor(u => u.BucketName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}