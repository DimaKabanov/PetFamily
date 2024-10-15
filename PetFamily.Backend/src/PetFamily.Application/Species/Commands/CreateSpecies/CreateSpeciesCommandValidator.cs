using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Species.ValueObjects;

namespace PetFamily.Application.Species.Commands.CreateSpecies;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}