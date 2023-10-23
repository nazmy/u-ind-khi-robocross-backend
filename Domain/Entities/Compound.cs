
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Entities
{
	public class Compound : Base
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("picName")]
        public string PicName { get; set; } = null!;

        [BsonElement("picPhoneNumber")]
        public string PicPhoneNumber { get; set; } = null!;

        [BsonElement("address")]
        public string Address { get; set; } = null!;

        [BsonElement("coordinates")]
        public GeoJsonPoint<GeoJson3DCoordinates> Coordinates { get; set; }

        [BsonElement("clientId")]
        public string ClientId { get; set; }
        
    }
}

