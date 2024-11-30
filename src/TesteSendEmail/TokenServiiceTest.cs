using SendEmail.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SendEmailTestes;
public class TokenServiceTests
{
    [Fact]
    public async Task GenerateToken_ShouldReturnValidJwtToken_ForAdminUser()
    {
        // Arrange
        var documento = "123456789";
        var role = "admin";
        var secretKey = "htr1rtj8tr185181*/*966jtrjrtjrtjrt"; // Deve ter no mínimo 16 caracteres
        var audience = "https://meuapp.com";
        var issuer = "https://issuer.com";

        Environment.SetEnvironmentVariable("SECRET_KEY", secretKey);
        Environment.SetEnvironmentVariable("Audience", audience);
        Environment.SetEnvironmentVariable("Issuer", issuer);

        var tokenService = new TokenService();

        // Act
        var token = await tokenService.GenerateToken(documento, true);

        // Assert
        ValidateToken(token, documento, role, secretKey, issuer, audience);
    }

    private void ValidateToken(string token, string documento, string role, string secretKey, string issuer, string audience)
    {
        Assert.NotNull(token);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var validationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        Assert.NotNull(validatedToken);
        Assert.IsType<JwtSecurityToken>(validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        Assert.Equal(documento, principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal(role, principal.FindFirst(ClaimTypes.Role)?.Value);
    }
}
