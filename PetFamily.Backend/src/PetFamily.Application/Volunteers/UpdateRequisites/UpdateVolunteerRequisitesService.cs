using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public class UpdateVolunteerRequisitesService(
    IVolunteersRepository volunteersRepository,
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
            .Select(r => Requisite.Create(r.Name, r.Description).Value);
        
        var requisiteList = new RequisiteList(requisites);

        volunteerResult.Value.UpdateRequisiteList(requisiteList);
        
        var result = await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Updated volunteer requisites with id: {volunteerId}", volunteerId);
        
        return result;
    }
}