using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Timeline : Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("unitId")]
    public string UnitId { get; set; }
    
    [BsonElement("timelineDetailsIds")]
    public string[] TimelineDetailsIds { get; set; }
    
  
}