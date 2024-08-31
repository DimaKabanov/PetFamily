using PetFamily.Domain.Models.Volunteers;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);
}