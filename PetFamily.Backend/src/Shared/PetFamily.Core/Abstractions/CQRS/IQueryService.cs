namespace PetFamily.Core.Abstractions.CQRS;

public interface IQueryService<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Handle(TQuery query, CancellationToken ct);
}