using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Options;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider(IOptions<JwtOptions> options) : ITokenProvider
{
    public string MakeAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = [
            new(CustomClaims.Sub, user.Id.ToString()),
            new(CustomClaims.Email, user.Email ?? "")];

        var jwtToken = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(options.Value.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}