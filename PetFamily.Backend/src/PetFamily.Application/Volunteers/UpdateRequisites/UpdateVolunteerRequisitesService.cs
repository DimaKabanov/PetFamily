using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public class UpdateVolunteerRequisitesService(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, Error>> Update(
        UpdateVolunteerRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var requisites = request.Dto.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList();

        volunteerResult.Value.UpdateRequisiteList(requisites);

        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation("Updated volunteer requisites with id: {volunteerId}", volunteerId);
        
        return volunteerResult.Value.Id.Value;
    }
}