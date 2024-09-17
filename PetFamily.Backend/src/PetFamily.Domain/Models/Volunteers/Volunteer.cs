using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer : Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    private Volunteer(VolunteerId id) : base(id)
    {
    }
    
    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description description,
        Experience experience,
        Phone phone,
        SocialNetworkList socialNetworkList,
        RequisiteList requisiteList) : base(id)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        Phone = phone;
        SocialNetworkList = socialNetworkList;
        RequisiteList = requisiteList;
    }
    
    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public Experience Experience { get; private set; }
    
    public Phone Phone { get; private set; }

    public RequisiteList RequisiteList { get; private set; }
    
    public SocialNetworkList SocialNetworkList { get; private set; }

    public List<Pet> Pets { get; private set; } = [];
    
    public int PetsNeedsHelpCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
    
    public int PetsSearchHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.SearchAHome);
    
    public int PetsFoundHomeCount() => Pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundAHome);

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Experience experience,
        Phone phone)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        Phone = phone;
    }
    
    public void UpdateSocialNetworkList(SocialNetworkList socialNetworkList)
    {
        SocialNetworkList = socialNetworkList;
    }
    
    public void UpdateRequisiteList(RequisiteList requisiteList)
    {
        RequisiteList = requisiteList;
    }

    public void Delete()
    {
        if (!_isDeleted)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }
}