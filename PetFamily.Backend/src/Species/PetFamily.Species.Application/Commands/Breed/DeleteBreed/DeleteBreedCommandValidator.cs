using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Commands.Breed.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(d => d.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}