using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    public string Email { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
}