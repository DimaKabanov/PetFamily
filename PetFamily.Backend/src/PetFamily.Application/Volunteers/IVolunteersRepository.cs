using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);
    
    Guid Save(Volunteer volunteer, CancellationToken cancellationToken);
    
    Guid Delete(Volunteer volunteer, CancellationToken cancellationToken);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);
}