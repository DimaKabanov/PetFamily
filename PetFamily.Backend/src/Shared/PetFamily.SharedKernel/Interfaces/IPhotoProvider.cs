using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.PhotoProvider;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.SharedKernel.Interfaces;

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