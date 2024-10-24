using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddSpeciesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}