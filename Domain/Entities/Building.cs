using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Entities
{
	public class Building : Base
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

        [BsonElement("compoundId")]
        public string CompoundId { get; set; }
        
        [BsonElement("clientId")]
        public string ClientId { get; set; }
    }
}

