using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.Pet.DeletePhotos;

public class DeletePhotosCommandValidator : AbstractValidator<DeletePhotosCommand>
{
    public DeletePhotosCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}