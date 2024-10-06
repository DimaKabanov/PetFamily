using FluentValidation;
using PetFamily.Application.Dto;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.AddPhotoToPet;

public class UploadPhotoToPetCommandValidator : AbstractValidator<UploadPhotoToPetCommand>
{
    public UploadPhotoToPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(u => u.Photos).SetValidator(new UploadPhotoDtoValidator());
    }
}

public class UploadPhotoDtoValidator : AbstractValidator<UploadPhotoDto>
{
    public UploadPhotoDtoValidator()
    {
        RuleFor(u => u.PhotoName)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.Content).Must(s => s.Length < 5000000);
    }
}