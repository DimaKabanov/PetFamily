using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Upload;

public class UploadFileService(IFileProvider fileProvider)
{
    public async Task<Result<string, Error>> Upload(
        UploadFileRequest request,
        CancellationToken cancellationToken)
    {
        return await fileProvider.UploadFile(request.stream, request.bucketName, request.fileName, cancellationToken);
    }
}