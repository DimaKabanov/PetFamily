using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.AddPetToVolunteer;
using PetFamily.Application.Volunteers.AddPhotoToPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Queries.GetVolunteers;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

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