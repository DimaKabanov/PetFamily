using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksRequestValidator : AbstractValidator<UpdateVolunteerSocialNetworksRequest>
{
    public UpdateVolunteerSocialNetworksRequestValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateVolunteerSocialNetworksDtoValidator : AbstractValidator<UpdateVolunteerSocialNetworksDto>
{
    public UpdateVolunteerSocialNetworksDtoValidator()
    {
        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Url));
    }
}