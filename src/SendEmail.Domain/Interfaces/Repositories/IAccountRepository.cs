using SendEmail.Domain.Model;

namespace SendEmail.Domain.Interfaces.Repositories;

public interface IAccountRepository
{
    Task CriarConta(Account account);
    Task<Account> BuscarContaPorDocumentoOuEmail(string documento, string email);
    Task<Account> BuscarContaPorIdentificador(string identificadorConta);
    Task AtualizarStatusConta(string identificadorConta, bool statusAtivo);
}
