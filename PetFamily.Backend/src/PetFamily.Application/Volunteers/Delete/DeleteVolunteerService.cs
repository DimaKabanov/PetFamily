using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerService(
    IVolunteersRepository volunteersRepository,
    ILogger<DeleteVolunteerService> logger)
{
    public async Task<Result<Guid, Error>> Delete(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var result = await volunteersRepository.Delete(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Deleted volunteer with id: {volunteerId}", volunteerId);

        return result;
    }
}