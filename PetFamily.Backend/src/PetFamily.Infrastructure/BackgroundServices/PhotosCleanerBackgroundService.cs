using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Messaging;
using PetFamily.Application.PhotoProvider;

namespace PetFamily.Infrastructure.BackgroundServices;

public class PhotosCleanerBackgroundService(
    IServiceScopeFactory scopeFactory,
    IMessageQueue<IEnumerable<PhotoInfo>> messageQueue,
    ILogger<PhotosCleanerBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope(); 

        var photoProvider = scope.ServiceProvider.GetRequiredService<IPhotoProvider>();

        while (!ct.IsCancellationRequested)
        {
            var photoInfos = await messageQueue.ReadAsync(ct);

            foreach (var photoInfo in photoInfos)
            {
                await photoProvider.RemoveFile(photoInfo, ct);
            }
        }

        await Task.CompletedTask;
    }
}