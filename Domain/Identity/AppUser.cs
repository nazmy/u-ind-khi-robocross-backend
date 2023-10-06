using AspNetCore.Identity.MongoDbCore.Models;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace domain.Repositories;

[CollectionName("Users")]
public class AppUser : MongoIdentityUser<ObjectId>
{
    private readonly IMongoCollection<User> _user;

    [BsonElement("ClientId")]
    public string? ClientId { get; set; }
    
    [BsonElement("Fullname")]
    public string? Fullname { get; set; }

}