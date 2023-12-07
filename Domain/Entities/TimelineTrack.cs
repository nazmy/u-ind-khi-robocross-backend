using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Domain.Entities;

public class TimelineTrack
{
    public TimelineTrack()
    {
    }

    [BsonElement("sceneObjectId")]
    public string sceneObjectId { get; set; }
    
    public RobotArmTimelineClip[] Clips { get; set; }
}