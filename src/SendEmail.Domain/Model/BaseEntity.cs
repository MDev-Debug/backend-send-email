using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SendEmail.Domain.Model;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
}
