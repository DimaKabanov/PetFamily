using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    string UserName) : ICommand;