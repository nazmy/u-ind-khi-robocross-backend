using System;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Entities
{
	public class Client : Base
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
    }
}

