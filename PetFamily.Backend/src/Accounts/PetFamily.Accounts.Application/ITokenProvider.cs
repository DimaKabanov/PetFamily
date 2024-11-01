using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    string MakeAccessToken(User user);
}