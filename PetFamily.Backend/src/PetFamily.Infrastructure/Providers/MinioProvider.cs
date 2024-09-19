using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger) : IFileProvider
{
    public async Task<Result<string, Error>> UploadFile(
        Stream stream,
        string bucketName,
        string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);
            var bucketExist = await minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
        
            if (!bucketExist)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(fileName);

            var result = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            
            return result.ObjectName;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to upload file");
            
            return Error.Failure("file.upload", "Failed to upload file");
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