using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Domain;

namespace PetFamily.Volunteers.Application;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken ct);
    
    Guid Save(Volunteer volunteer, CancellationToken ct);
    
    Guid Delete(Volunteer volunteer, CancellationToken ct);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken ct);
}