using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);
    
    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken);
    
    Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);
}