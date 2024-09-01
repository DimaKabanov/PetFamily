using CSharpFunctionalExtensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;

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

        var fullName = FullName.Create(
            request.FullName.Name,
            request.FullName.Surname,
            request.FullName.Patronymic
        );

        if (fullName.IsFailure)
            return fullName.Error;
            
        var description = Description.Create(request.Description);
        
        if (description.IsFailure)
            return description.Error;

        var experience = Experience.Create(request.Experience);
        
        if (experience.IsFailure)
            return experience.Error;

        var phone = Phone.Create(request.Phone);
        
        if (phone.IsFailure)
            return phone.Error;

        var socialNetworks = request.SocialNetworks.Select(
            s => SocialNetwork.Create(s.Title, s.Url).Value
        );

        var requisites = request.Requisites.Select(
            r => Requisite.Create(r.Name, r.Description).Value
        );

        var details = Detail.Create(socialNetworks, requisites);

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