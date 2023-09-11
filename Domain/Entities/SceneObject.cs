using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class SceneObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("libraryUrl")]
    public string LibraryUrl { get; set; } = null!;

    [BsonElement("robots")] 
    public List<Robot> robots { get; set; } = new List<Robot>();
}