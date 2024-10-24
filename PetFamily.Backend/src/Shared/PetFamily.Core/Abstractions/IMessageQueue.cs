namespace PetFamily.Core.Abstractions;

public interface IMessageQueue<TMessage>
{
    Task WriteAsync(TMessage message, CancellationToken ct);

    Task<TMessage> ReadAsync(CancellationToken ct);
}