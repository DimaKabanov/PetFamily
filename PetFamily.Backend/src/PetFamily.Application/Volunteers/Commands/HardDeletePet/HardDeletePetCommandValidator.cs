using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.Volunteers.Commands.SoftDeletePet;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.HardDeletePet;

public class HardDeletePetCommandValidator : AbstractValidator<HardDeletePetCommand>
{
    public HardDeletePetCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}