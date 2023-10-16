using AspNetCore.Identity.MongoDbCore.Models;
using Mongo.Migration.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;


public class Role : Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }

    public DocumentVersion Version { get; set; }
}