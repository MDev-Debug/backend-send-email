using MongoDB.Driver;
using SendEmail.Domain.Interfaces.Repositories;
using SendEmail.Domain.Model;
using SendEmail.Infra.Data.Data;

namespace SendEmail.Infra.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IMongoCollection<Account> _collection;

    public AccountRepository(IMongoDbContext dbContext)
    {
        _collection = dbContext.Collection<Account>();
    }

    public async Task AtualizarStatusConta(string identificadorConta, bool statusAtivo)
    {
        var accont = await _collection.FindAsync(x => x.Identificador == identificadorConta || x.Id == identificadorConta);

        var filter = Builders<Account>.Filter.Eq(x => x.Identificador, identificadorConta) |
                 Builders<Account>.Filter.Eq(x => x.Id, identificadorConta);

        var update = Builders<Account>.Update.Set(x => x.UsuarioAtivo, statusAtivo);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<Account> BuscarContaPorDocumentoOuEmail(string documento, string email)
    {
        var account = await _collection.Find(x => x.Documento == documento || x.Email == email).FirstOrDefaultAsync();
        return account;
    }

    public async Task<Account> BuscarContaPorIdentificador(string identificadorConta)
    {
        var account = await _collection.Find(x => x.Id == identificadorConta || x.Identificador == identificadorConta).FirstOrDefaultAsync();
        return account;
    }

    public async Task CriarConta(Account account)
        => await _collection.InsertOneAsync(account);
}
