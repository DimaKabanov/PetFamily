using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.LoginUser;

public class LoginUserService(
    ITokenProvider tokenProvider,
    UserManager<User> userManager) : ICommandService<string, LoginUserCommand>
{
    public async Task<Result<string, ErrorList>> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Errors.General.NotFound().ToErrorList();

        var passwordConfirmed = await userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
            return Errors.User.InvalidCredentials().ToErrorList();

        var token = tokenProvider.MakeAccessToken(user);

        return token;
    }
}