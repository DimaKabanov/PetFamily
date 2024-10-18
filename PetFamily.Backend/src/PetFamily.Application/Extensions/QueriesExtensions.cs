using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;
using System.Linq.Expressions;

namespace PetFamily.Application.Extensions;

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
        this IQueryable<T> sourse,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? sourse.Where(predicate) : sourse;
    }
}