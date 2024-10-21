using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core.Abstractions;
using PetFamily.Core.PhotoProvider;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Infrastructure.Providers;

public class MinioProvider(
    IMinioClient minioClient,
    ILogger<MinioProvider> logger) : IPhotoProvider
{
    private const int MAX_PARALLELISM = 10;
    
    public async Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFiles(
        IEnumerable<PhotoData> photosData,
        CancellationToken ct)
    {
        var semaphore = new SemaphoreSlim(MAX_PARALLELISM);
        var photoList = photosData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(photoList, ct);
            
            var tasks = photoList.Select(async photo => 
                await PutObject(photo, semaphore, ct));

            var pathsResult = await Task.WhenAll(tasks);
            
            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;
            
            var results = pathsResult.Select(p => p.Value).ToList();
            
            return results;
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Fail to upload photos in minio, photos amount: {amount}",
                photoList.Count);

            return Error.Failure("photo.upload", "Fail to upload photos in minio");
        }
    }

    public async Task<UnitResult<Error>> RemoveFile(PhotoInfo photoInfo, CancellationToken ct)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(photoInfo.BucketName)
                .WithObject(photoInfo.PhotoPath.Path);

            var objectStat = await minioClient.StatObjectAsync(statObjectArgs, ct);
            if (objectStat is null)
                return Result.Success<Error>();

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(photoInfo.BucketName)
                .WithObject(photoInfo.PhotoPath.Path);

            await minioClient.RemoveObjectAsync(removeObjectArgs, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to delete file");
            return Error.Failure("file.delete", "Failed to delete file");
        }
        
        return Result.Success<Error>();
    }

    public async Task<Result<string, Error>> DownloadFile(string bucketName, string fileName)
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithExpiry(60 * 60 * 24);

            var url = await minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

            return url;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to download file");

            return Error.Failure("file.download", "Failed to download file");
        }
    }

    private async Task<Result<PhotoPath, Error>> PutObject(
        PhotoData photoData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken ct)
    {
        await semaphoreSlim.WaitAsync(ct);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(photoData.Info.BucketName)
            .WithStreamData(photoData.Stream)
            .WithObjectSize(photoData.Stream.Length)
            .WithObject(photoData.Info.PhotoPath.Path);

        try
        {
            await minioClient.PutObjectAsync(putObjectArgs, ct);

            return photoData.Info.PhotoPath;
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Fail to upload photo in minio with path {path} in bucket {bucket}",
                photoData.Info.PhotoPath.Path,
                photoData.Info.BucketName);

            return Error.Failure("photo.upload", "Fail to upload photo in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task IfBucketsNotExistCreateBucket(IEnumerable<PhotoData> photosData, CancellationToken ct)
    {
        HashSet<string> bucketNames = [..photosData.Select(p => p.Info.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);

            var bucketExist = await minioClient
                .BucketExistsAsync(bucketExistArgs, ct);

            if (bucketExist) continue;
            
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            await minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }
    }
}