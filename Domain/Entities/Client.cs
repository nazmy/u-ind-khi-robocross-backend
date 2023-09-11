using System;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Entities
{
	public class Client
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("emailAddress")]
        public string EmailAddress { get; set; } = null!;

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;
        
        [BsonElement("clientType")]
        public ClientTypeEnum ClientType { get; set; } = 0;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [BsonElement("lastUpdatedBy")]
        public string LastUpdatedBy { get; set; } = null!;

        [BsonElement("lastUpdatedAt")]
        public DateTimeOffset? LastUpdatedAt { get; set; }

        public void CreateChangesTime(Client client)
        {
            client.CreatedAt = DateTimeOffset.UtcNow;
            client.CreatedBy = "Creator";
        }

        public void UpdateChangesTime(Client client)
        {
            client.LastUpdatedAt = DateTimeOffset.UtcNow;
            client.LastUpdatedBy = "Updater";
        }
    }
}

