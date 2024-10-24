using System.Data;

namespace PetFamily.Species.Application;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken);

    Task SaveChanges(CancellationToken cancellationToken);
}