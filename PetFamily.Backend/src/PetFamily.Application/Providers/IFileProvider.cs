using CSharpFunctionalExtensions;
using PetFamily.Application.PhotoProvider;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFiles(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken);
    
    Task<Result<string, Error>> DeleteFile(
        string bucketName,
        string fileName,
        CancellationToken cancellationToken);
    
    Task<Result<string, Error>> DownloadFile(
        string bucketName,
        string fileName);
}