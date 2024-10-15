using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken ct);
    
    Guid Save(Volunteer volunteer, CancellationToken ct);
    
    Guid Delete(Volunteer volunteer, CancellationToken ct);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken ct);
}