using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.Pet.SetMainPhoto;

public class SetMainPhotoCommandValidator : AbstractValidator<SetMainPhotoCommand>
{
    public SetMainPhotoCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(s => s.PhotoPath).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}