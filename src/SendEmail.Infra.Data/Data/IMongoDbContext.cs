using MongoDB.Driver;

namespace SendEmail.Infra.Data.Data;

public interface IMongoDbContext : IDisposable
{
    IMongoCollection<T> Collection<T>();
}
