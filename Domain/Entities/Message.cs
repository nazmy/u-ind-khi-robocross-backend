using System.Numerics;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Message : Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("ownerId")]
    public string OwnerId { get; set; }
    
    [BsonElement("topicId")]
    public string TopicId { get; set; }
    
    [BsonElement("topicType")]
    public MessageTopicTypeEnum TopicType { get; set; } = 0;
    
    [BsonElement("messageType")]
    public MessageTypeEnum MessageType { get; set; } = 0;
    
    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("body")]
    public string Body { get; set; }
    
    public Vector3[] Points { get; set; }
    
    public Vector3[] Normals { get; set; }
    
    public CameraState? CameraState { get; set; }
    
    public string[] AttachmentUrls { get; set; }

    [BsonElement("unread")] 
    public bool Unread { get; set; } = true;
}