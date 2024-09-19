using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete;

public class DeleteFileService(IFileProvider fileProvider)
{
    public async Task<Result<string, Error>> Delete(
        DeleteFileRequest request,
        CancellationToken cancellationToken)
    {
        return await fileProvider.DeleteFile(
            request.BucketName,
            request.FileName,
            cancellationToken);
    }
}