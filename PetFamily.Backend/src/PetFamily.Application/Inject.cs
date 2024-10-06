using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Commands.AddPetToVolunteer;
using PetFamily.Application.Volunteers.Commands.AddPhotoToPet;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.Delete;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;
using PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;
using PetFamily.Application.Volunteers.Queries.GetVolunteers;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerService>();
        services.AddScoped<UpdateVolunteerMainInfoService>();
        services.AddScoped<UpdateVolunteerSocialNetworksService>();
        services.AddScoped<UpdateVolunteerRequisitesService>();
        services.AddScoped<DeleteVolunteerService>();
        services.AddScoped<AddPetToVolunteerService>();
        services.AddScoped<UploadPhotoToPetService>();
        services.AddScoped<GetVolunteersService>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}