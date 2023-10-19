using System;
using System.Numerics;
using System.Text.Json.Serialization;
using domain.Dto;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
	public class MessageResponse : BaseDto
	{
		public string Id { get; set; }
		
		public string? OwnerId { get; set; }

        public string TopicId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string TopicType { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string MessageType { get; set; }
        
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public Vector3[]? Points { get; set; }
    
        public Vector3[]? Normals { get; set; }
    
        public string? CameraState { get; set; }
    
        public string[]? AttachmentUrls { get; set; }
        
        public bool Unread { get; set; }
    }
}

