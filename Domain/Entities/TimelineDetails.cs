using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class TimelineDetails : Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("timelineTracks")]
    public TimelineTrack[] Tracks { get; set; }
}