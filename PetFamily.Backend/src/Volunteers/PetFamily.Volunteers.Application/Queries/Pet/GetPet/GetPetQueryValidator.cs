using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPet;

public class GetPetQueryValidator : AbstractValidator<GetPetQuery>
{
    public GetPetQueryValidator()
    {
        RuleFor(g => g.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}