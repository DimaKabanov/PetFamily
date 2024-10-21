using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Commands.Species.DeleteSpecies;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(d => d.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}