using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public record RobotArmTimelineClip : ITimelineClip
{
    [BsonElement("type")] public string Type { get; } = nameof(RobotArmTimelineClip);
    
    [BsonElement("startTime")]
    public float StartTime { get; set; }
    
    [BsonElement("endTime")]
    public float EndTime { get; set; }
    
    [BsonElement("objectAction")]
    public ObjectAction ObjectAction { get; set; }
    
    [BsonElement("destinationObjectId")] public string? DestinationObjectId { get; set; }

    [BsonElement("handlingObjectId")] public string? HandlingObjectId { get; set; }
}