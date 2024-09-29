using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.PhotoProvider;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(
    IMinioClient minioClient,
    ILogger<MinioProvider> logger) : IPhotoProvider
{
    private const int MAX_PARALLELISM = 10;
    
    public async Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFiles(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken)
    {
        var semaphore = new SemaphoreSlim(MAX_PARALLELISM);
        var photoList = photosData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(photoList, cancellationToken);
            
            var tasks = photoList.Select(async photo => 
                await PutObject(photo, semaphore, cancellationToken));

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
    
    private async Task<Result<PhotoPath, Error>> PutObject(
        PhotoData photoData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(photoData.BucketName)
            .WithStreamData(photoData.Stream)
            .WithObjectSize(photoData.Stream.Length)
            .WithObject(photoData.PhotoPath.Path);

        try
        {
            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return photoData.PhotoPath;
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Fail to upload photo in minio with path {path} in bucket {bucket}",
                photoData.PhotoPath.Path,
                photoData.BucketName);

            return Error.Failure("photo.upload", "Fail to upload photo in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..photosData.Select(p => p.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);

            var bucketExist = await minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist) continue;
            
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
    }
    
    public async Task<Result<string, Error>> DeleteFile(
        string bucketName,
        string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);
            
            await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            return fileName;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to delete file");

            return Error.Failure("file.delete", "Failed to delete file");
        }
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
}