using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Ids;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerService
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerService(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Create(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.NewId();

        var fullName = VolunteerFullName.Create(
            request.FullName.Name,
            request.FullName.Surname,
            request.FullName.Patronymic
        );

        if (fullName.IsFailure)
            return fullName.Error;
            
        var description = VolunteerDescription.Create(request.Description);
        
        if (description.IsFailure)
            return description.Error;

        var experience = VolunteerExperience.Create(request.Experience);
        
        if (experience.IsFailure)
            return experience.Error;

        var phone = VolunteerPhone.Create(request.Phone);
        
        if (phone.IsFailure)
            return phone.Error;

        var socialNetworksResults = request.SocialNetworks.Select(
            s => VolunteerSocialNetwork.Create(s.Title, s.Url)
        );

        if (socialNetworksResults.Any(s => s.IsFailure))
            return socialNetworksResults.FirstOrDefault(s => s.IsFailure).Error;

        var socialNetworks = socialNetworksResults.Select(
            s => s.Value);

        var requisitesResults = request.Requisites.Select(
            r => Requisite.Create(r.Name, r.Description)
        );
        
        if (requisitesResults.Any(r => r.IsFailure))
            return requisitesResults.FirstOrDefault(r => r.IsFailure).Error;
        
        var requisites = requisitesResults.Select(r => r.Value);

        var details = VolunteerDetail.Create(socialNetworks, requisites);

        var volunteer = new Volunteer(
            volunteerId,
            fullName.Value,
            description.Value,
            experience.Value,
            phone.Value,
            details
        );

        await _volunteersRepository.Add(volunteer, cancellationToken);

        return volunteerId.Value;
    }
}