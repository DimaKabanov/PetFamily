using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(u => u.AssistanceStatus)
            .Must(status => status is AssistanceStatus.NeedsHelp or AssistanceStatus.SearchAHome)
            .WithError(Errors.General.ValueIsInvalid());
    }
}