using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Entities
{
	public class Compound
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        public string PicName { get; set; } = null!;

        public string PicPhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string CreatedBy { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }

        public string LastUpdatedBy { get; set; } = null!;

        public DateTimeOffset LastUpdatedAt { get; set; }

        public void CreateChangesTime(Compound compound)
        {
            compound.CreatedAt = DateTimeOffset.UtcNow;
            compound.LastUpdatedAt = DateTimeOffset.UtcNow;
            compound.CreatedBy = "Creator";
        }

        public void UpdateChangesTime(Compound compound)
        {
            compound.LastUpdatedAt = DateTimeOffset.UtcNow;
            compound.LastUpdatedBy = "Updater";
        }
    }
}

