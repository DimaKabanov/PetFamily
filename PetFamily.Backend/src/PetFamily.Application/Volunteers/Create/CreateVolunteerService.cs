using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerService(
    IVolunteersRepository volunteersRepository,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, Error>> Create(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(
            request.FullName.Name,
            request.FullName.Surname,
            request.FullName.Patronymic).Value;
            
        var description = Description.Create(request.Description).Value;

        var experience = Experience.Create(request.Experience).Value;

        var phone = Phone.Create(request.Phone).Value;

        var socialNetworks = request.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value)
            .ToList();

        var requisites = request.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();
        
        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            experience,
            phone,
            socialNetworks,
            requisites);

        await volunteersRepository.Add(volunteer, cancellationToken);
        
        logger.LogInformation("Create volunteer with id: {volunteerId}", volunteerId);

        return volunteerId.Value;
    }
}