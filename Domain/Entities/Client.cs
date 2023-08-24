using System;
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

        [BsonElement("EmailAddress")]
        public string EmailAddress { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public string CreatedBy { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }

        public string LastUpdatedBy { get; set; } = null!;

        public DateTimeOffset LastUpdatedAt { get; set; }

        public void CreateChangesTime(Client client)
        {
            client.CreatedAt = DateTimeOffset.UtcNow;
            client.LastUpdatedAt = DateTimeOffset.UtcNow;
            client.CreatedBy = "Creator";
        }

        public void UpdateChangesTime(Client client)
        {
            client.LastUpdatedAt = DateTimeOffset.UtcNow;
            client.LastUpdatedBy = "Updater";
        }
    }
}

