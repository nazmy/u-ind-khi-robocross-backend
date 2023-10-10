using AspNetCore.Identity.MongoDbCore.Models;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace domain.Repositories;

[CollectionName("Roles")]
public class AppRole : MongoIdentityRole<ObjectId>
{
    private readonly IMongoCollection<Role> _roles;
    
    [BsonElement("Description")]
    public string Description { get; set; }
}