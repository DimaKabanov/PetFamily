using PetFamily.Application.Volunteers.AddPhotoToPet;

namespace PetFamily.API.Processors;

public class FormPhotoProcessor : IAsyncDisposable
{
    private readonly List<CreatePhotoCommand> _photoDtos = [];

    public List<CreatePhotoCommand> Process(IFormFileCollection photos)
    {
        foreach (var photo in photos)
        {
            var stream = photo.OpenReadStream();
            var photoDto = new CreatePhotoCommand(stream, photo.FileName);

            _photoDtos.Add(photoDto);
        }

        return _photoDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var photo in _photoDtos)
        {
            await photo.Content.DisposeAsync();
        }
    }
}