using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
    public class WriteModelWrapper<T>
    {
        public T Document { get; set; }
    }
    
    public class BulkUserMessageResponse
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
        
        public bool Unread { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedAt { get; set; }

        public string LastUpdatedBy { get; set; }

        public string LastUpdatedAt { get; set; }

    }
}