using SendEmail.Domain.DTOs;
using SendEmail.Domain.DTOs.Request;

namespace SendEmail.Domain.Interfaces.Services;

public interface IAccountService
{
    Task<Response> CriarConta(AccountRequestDTO conta);
    Task<Response> Login(LoginRequestDTO request);
    Task<Response> AtualizarStatusConta(string identificadorConta, bool statusAtivo);
}
