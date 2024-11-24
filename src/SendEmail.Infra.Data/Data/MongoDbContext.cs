using MongoDB.Driver;

namespace SendEmail.Infra.Data.Data;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;
    public IClientSessionHandle Session { get; set; }

    public MongoDbContext()
    {
        var conn = Environment.GetEnvironmentVariable("MongoConnection");
        var mongoClient = new MongoClient(conn);
        _database = mongoClient.GetDatabase("GatewayDB");
    }

    public IMongoCollection<T> Collection<T>()
    {
        return _database.GetCollection<T>(typeof(T).Name);
    }

    public void Dispose()
    {
        Session?.Dispose();
        GC.SuppressFinalize(this);
    }
}
