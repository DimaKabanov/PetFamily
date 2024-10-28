using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AccountsWriteDbContext>();

        services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AccountsWriteDbContext>();

        services.AddAuthorization();

        return services;
    }
}