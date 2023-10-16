using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Domain.Entities;

public class User : Base
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
}