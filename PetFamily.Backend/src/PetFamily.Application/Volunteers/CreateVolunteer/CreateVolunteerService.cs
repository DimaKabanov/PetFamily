using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volunteers;
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

        var fullName = FullName.Create(
            request.FullName.Name,
            request.FullName.Surname,
            request.FullName.Patronymic).Value;
            
        var description = Description.Create(request.Description).Value;

        var experience = Experience.Create(request.Experience).Value;

        var phone = Phone.Create(request.Phone).Value;

        var socialNetworks = request.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value);

        var requisites = request.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value);

        var details = new Detail(socialNetworks, requisites);

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            experience,
            phone,
            details);

        await _volunteersRepository.Add(volunteer, cancellationToken);

        return volunteerId.Value;
    }
}