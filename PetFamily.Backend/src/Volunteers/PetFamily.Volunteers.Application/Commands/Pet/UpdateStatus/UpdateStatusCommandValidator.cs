using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Enums;

namespace PetFamily.Volunteers.Application.Commands.Pet.UpdateStatus;

public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusCommandValidator()
    {
        RuleFor(u => u.AssistanceStatus)
            .Must(status => status is AssistanceStatus.NeedsHelp or AssistanceStatus.SearchAHome)
            .WithError(Errors.General.ValueIsInvalid());
    }
}