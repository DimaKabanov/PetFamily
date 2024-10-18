using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.DeletePhotosFromPet;

public class DeletePhotosFromPetCommandValidator : AbstractValidator<DeletePhotosFromPetCommand>
{
    public DeletePhotosFromPetCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}