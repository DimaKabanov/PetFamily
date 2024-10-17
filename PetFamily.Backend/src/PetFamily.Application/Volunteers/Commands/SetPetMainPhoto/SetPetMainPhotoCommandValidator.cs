using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.SetPetMainPhoto;

public class SetPetMainPhotoCommandValidator : AbstractValidator<SetPetMainPhotoCommand>
{
    public SetPetMainPhotoCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(s => s.PhotoPath).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}