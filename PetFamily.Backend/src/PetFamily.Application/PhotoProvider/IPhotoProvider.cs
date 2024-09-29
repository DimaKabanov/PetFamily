using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.PhotoProvider;

public interface IPhotoProvider
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