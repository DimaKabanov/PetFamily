using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksCommandValidator : AbstractValidator<UpdateVolunteerSocialNetworksCommand>
{
    public UpdateVolunteerSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Url));
    }
}