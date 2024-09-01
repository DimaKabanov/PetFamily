using PetFamily.Domain.Models;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerService
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerService(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Guid> Create(CreateVolunteerRequest request, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(
            request.FullName.Name,
            request.FullName.Surname,
            request.FullName.Patronymic
        );

        var description = Description.Create(request.Description);

        var experience = Experience.Create(request.Experience);

        var phone = Phone.Create(request.Phone);

        var socialNetworks = request.SocialNetworks.Select(
            s => SocialNetwork.Create(s.Title, s.Url)
        );

        var requisites = request.Requisites.Select(
            r => Requisite.Create(r.Name, r.Description)
        );

        var details = Detail.Create(socialNetworks, requisites);

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            experience,
            phone,
            details
        );

        await _volunteersRepository.Add(volunteer, cancellationToken);

        return volunteerId.Value;
    }
}