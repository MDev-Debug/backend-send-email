using Microsoft.IdentityModel.Tokens;
using SendEmail.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SendEmail.Domain.Services;

public class TokenService : ITokenService
{
    private readonly string _chaveSecreta = "";
    public TokenService()
    {
        var keyJwt = Environment.GetEnvironmentVariable("SECRET_KEY");
        if (!string.IsNullOrEmpty(keyJwt))
            _chaveSecreta = keyJwt;
    }

    public async Task<string> GenerateToken(string documento, bool admin)
    {
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, documento),
                    new Claim(ClaimTypes.Role, admin == true ? "admin" : "user")
                }),
            SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey
                            (Encoding.ASCII.GetBytes(_chaveSecreta)),
                             SecurityAlgorithms.HmacSha256Signature),
            Audience = Environment.GetEnvironmentVariable("Audience"),
            Issuer = Environment.GetEnvironmentVariable("Issuer"),
            Expires = DateTime.UtcNow.AddHours(1)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }
}
