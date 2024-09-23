using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FIleProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(
    IMinioClient minioClient,
    ILogger<MinioProvider> logger) : IFileProvider
{
    public const int MAX_PARALLELISM = 10;
    public async Task<UnitResult<Error>> UploadFiles(
        FileData fileData,
        CancellationToken cancellationToken)
    {
        var semaphore = new SemaphoreSlim(MAX_PARALLELISM);
        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(fileData.BucketName);
            var bucketExist = await minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (!bucketExist)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(fileData.BucketName);
                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            List<Task> tasks = [];
            foreach (var file in fileData.Files)
            {
                await semaphore.WaitAsync(cancellationToken);

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(file.Stream)
                    .WithObjectSize(file.Stream.Length)
                    .WithObject(file.ObjectName);

                var task = minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                semaphore.Release();

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to upload file");
            return Error.Failure("file.upload", "Failed to upload file");
        }
        finally
        {
            semaphore.Release();
        }

        return Result.Success<Error>();
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