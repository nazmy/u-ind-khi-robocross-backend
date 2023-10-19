using System;
using System.Numerics;
using System.Text.Json.Serialization;
using domain.Dto;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
	public class CreateMessageInput
	{
		public string? OwnerId { get; set; }

        public string TopicId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageTopicTypeEnum TopicType { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageTypeEnum MessageType { get; set; }
        
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public Vector3[]? Points { get; set; }
    
        public Vector3[]? Normals { get; set; }
    
        public string? CameraState { get; set; }
    
        public string[]? AttachmentUrls { get; set; }
    }
}

