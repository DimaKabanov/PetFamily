using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public static class Inject
{
    public static IServiceCollection AddVolunteerPresentation(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersContract, VolunteersContract>();

        return services;
    } 
}