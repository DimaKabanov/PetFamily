using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.Breed.GetBreedsBySpecies;

public class GetBreedsBySpeciesQueryValidator : AbstractValidator<GetBreedsBySpeciesQuery>
{
    public GetBreedsBySpeciesQueryValidator()
    {
        RuleFor(g => g.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}