using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Abstractions;

public interface IQueryService<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Run(TQuery query, CancellationToken ct);
}