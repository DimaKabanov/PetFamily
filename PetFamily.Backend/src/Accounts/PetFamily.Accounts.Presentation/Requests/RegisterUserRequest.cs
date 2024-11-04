using PetFamily.Accounts.Application.Commands.RegisterUser;

namespace PetFamily.Accounts.Presentation.Requests;

public record RegisterUserRequest(
    string Email,
    string Password,
    string UserName)
{
    public RegisterUserCommand ToCommand() => new(Email, Password, UserName);
};