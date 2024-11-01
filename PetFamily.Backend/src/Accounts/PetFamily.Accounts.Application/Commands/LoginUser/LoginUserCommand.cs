using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Accounts.Application.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand;