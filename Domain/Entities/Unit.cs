using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Unit
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; } = null!;
    
    [BsonElement("pics")]
    public string[] Pics { get; set; } = null!;
    
    [BsonElement("sceneObjects")]
    public List<SceneObject> SceneObjects { get; set; } = new List<SceneObject>();
}