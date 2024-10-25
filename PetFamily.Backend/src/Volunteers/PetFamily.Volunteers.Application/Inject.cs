using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application;

public static class Inject
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(ICommandService<,>), typeof(ICommandService<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes =>
                classes.AssignableTo(typeof(IQueryService<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}