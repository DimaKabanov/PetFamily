using CSharpFunctionalExtensions;
using PetFamily.Application.FIleProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<Error>> UploadFiles(
        FileData fileData,
        CancellationToken cancellationToken);
    
    Task<Result<string, Error>> DeleteFile(
        string bucketName,
        string fileName,
        CancellationToken cancellationToken);
    
    Task<Result<string, Error>> DownloadFile(
        string bucketName,
        string fileName);
}