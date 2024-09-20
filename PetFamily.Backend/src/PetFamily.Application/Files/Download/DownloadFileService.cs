using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Download;

public class DownloadFileService(IFileProvider fileProvider)
{
    public async Task<Result<string, Error>> Download(DownloadFileRequest request)
    {
        return await fileProvider.DownloadFile(request.BucketName, request.FileName);
    }
}