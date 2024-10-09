using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Abstractions;

public interface ICommandService<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> Handle(TCommand command, CancellationToken ct);
}

public interface ICommandService<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> Handle(TCommand command, CancellationToken ct);
}