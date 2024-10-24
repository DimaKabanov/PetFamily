using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Models;

namespace PetFamily.Core.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source, int page, int pageSize, CancellationToken ct)
    {
        var totalCount = await source.CountAsync(ct);
        
        var items = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        return new PagedList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}