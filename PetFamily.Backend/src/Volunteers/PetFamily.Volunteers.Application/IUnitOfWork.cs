using System.Data;

namespace PetFamily.Volunteers.Application;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken);

    Task SaveChanges(CancellationToken cancellationToken);
}