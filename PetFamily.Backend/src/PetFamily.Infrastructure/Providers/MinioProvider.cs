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
}