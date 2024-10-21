using CSharpFunctionalExtensions;
using PetFamily.Core.PhotoProvider;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Core.Abstractions;

public interface IPhotoProvider
{
    Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFiles(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> RemoveFile(
        PhotoInfo photoInfo,
        CancellationToken cancellationToken);
    
    Task<Result<string, Error>> DownloadFile(
        string bucketName,
        string fileName);
}