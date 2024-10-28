using Serilog;
using Serilog.Events;

namespace PetFamily.Web.Extensions;

public static class LoggerExtension
{
    public static void ConfigureAppLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq(builder.Configuration.GetConnectionString("SeqPath") 
                         ?? throw new ArgumentNullException("SeqPath"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();
    }
}