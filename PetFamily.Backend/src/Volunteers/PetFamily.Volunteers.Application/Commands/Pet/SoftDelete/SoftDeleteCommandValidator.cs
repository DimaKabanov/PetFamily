using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.Pet.SoftDelete;

public class SoftDeleteCommandValidator : AbstractValidator<SoftDeleteCommand>
{
    public SoftDeleteCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}