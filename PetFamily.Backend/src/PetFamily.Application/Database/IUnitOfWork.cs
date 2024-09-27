using System.Data;

namespace PetFamily.Application.Database;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken);

    Task SaveChanges(CancellationToken cancellationToken);
}