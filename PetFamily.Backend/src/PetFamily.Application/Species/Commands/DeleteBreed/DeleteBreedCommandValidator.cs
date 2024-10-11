using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(d => d.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}