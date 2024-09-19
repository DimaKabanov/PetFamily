using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete;

public class DeleteFileRequestValidator : AbstractValidator<DeleteFileRequest>
{
    public DeleteFileRequestValidator()
    {
        RuleFor(u => u.BucketName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}