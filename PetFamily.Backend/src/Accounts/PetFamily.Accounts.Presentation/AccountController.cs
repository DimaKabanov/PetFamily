using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation;

public class AccountController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserService service,
        CancellationToken ct)
    {
        var result = await service.Handle(request.ToCommand(), ct);
        return result.IsFailure ? result.Error.ToResponse() : Ok(result);
    }
}