using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.Species.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}