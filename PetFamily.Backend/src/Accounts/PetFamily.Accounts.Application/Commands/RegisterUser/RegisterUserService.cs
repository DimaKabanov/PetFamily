using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public class RegisterUserService(
    UserManager<User> userManager) : ICommandService<RegisterUserCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var existingUser = await userManager.FindByEmailAsync(command.Email);
        if (existingUser is not null)
            return Errors.General.ValueAlreadyExisting(existingUser.Id).ToErrorList();

        var user = new User
        {
            Email = command.Email,
            UserName = command.UserName
        };

        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));
            return new ErrorList(errors);
        }

        return Result.Success<ErrorList>();
    }
}