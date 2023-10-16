using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Base
{
    [BsonElement("isDeleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("createdBy")]
    public string CreatedBy { get; set; } = null!;

    [BsonElement("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [BsonElement("lastUpdatedBy")]
    public string LastUpdatedBy { get; set; } = null!;

    [BsonElement("lastUpdatedAt")]
    public DateTimeOffset? LastUpdatedAt { get; set; }
    
    public void CreateChangesTime<T>(T entity, string username)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = username;
    }

    public void UpdateChangesTime<T>(T entity, string username)
    {
        LastUpdatedAt = DateTimeOffset.UtcNow;
        LastUpdatedBy = username;
    }
}