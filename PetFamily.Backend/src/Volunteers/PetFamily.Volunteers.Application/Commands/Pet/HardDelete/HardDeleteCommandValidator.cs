using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.Pet.HardDelete;

public class HardDeleteCommandValidator : AbstractValidator<HardDeleteCommand>
{
    public HardDeleteCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}