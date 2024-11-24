namespace SendEmail.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<string> GenerateToken(string documento, bool admin);
}
