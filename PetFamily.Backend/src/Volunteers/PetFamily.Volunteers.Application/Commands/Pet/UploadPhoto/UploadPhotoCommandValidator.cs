using FluentValidation;
using PetFamily.Core.Dto;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.Pet.UploadPhoto;

public class UploadPhotoCommandValidator : AbstractValidator<UploadPhotoCommand>
{
    public UploadPhotoCommandValidator()
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