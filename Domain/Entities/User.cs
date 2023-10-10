using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Domain.Entities;

public class User 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("email")]
    public string Email { get; set; } = null!;
    
    [BsonElement("userName")]
    public string Username { get; set; } = null!;
    
    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = null!;
    
    [BsonElement("lockoutEnabled")]
    public bool lockoutEnabled { get; set; } = false;
   
    [BsonElement("accessFailedCount")]
    public int AccessFailedCount { get; set; } = 0;
    
    [BsonElement("roleId")]
    public string RoleId { get; set; } = null!;
    
    [BsonElement("clientId")]
    public string? ClientId { get; set; } = null!;
    
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
    
    public void CreateChangesTime(User user)
    {
        user.CreatedAt = DateTimeOffset.UtcNow;
        user.CreatedBy = "Creator";
    }

    public void UpdateChangesTime(User user)
    {
        user.LastUpdatedAt = DateTimeOffset.UtcNow;
        user.LastUpdatedBy = "Updater";
    }
}