using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Species.Domain.Breeds.ValueObjects;

namespace PetFamily.Species.Application.Commands.Breed.AddToSpecies;

public class AddToSpeciesCommandValidator : AbstractValidator<AddToSpeciesCommand>
{
    public AddToSpeciesCommandValidator()
    {
        RuleFor(a => a.Name).MustBeValueObject(Name.Create);
    }
}