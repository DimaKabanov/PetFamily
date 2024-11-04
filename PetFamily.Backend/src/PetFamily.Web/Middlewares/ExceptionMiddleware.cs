using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Web.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var responseError = Error.Failure("server.internal", e.Message);
            var envelope = Envelope.Error(responseError);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}
