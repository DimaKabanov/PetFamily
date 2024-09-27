using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}