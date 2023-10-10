using AspNetCore.Identity.MongoDbCore.Models;
using Mongo.Migration.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;


public class Role 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }
    
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

    public DocumentVersion Version { get; set; }
}