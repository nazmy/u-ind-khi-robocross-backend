
using System;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Entities
{
	public class Line
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;
        
        [BsonElement("applications")]
        public uint Applications { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("scannedSceneGLBUrl")]
        public string ScannedSceneGLBUrl { get; set; } = null!;

        [BsonElement("status")]
        public LineStatusEnum Status { get; set; } = null!;

        [BsonElement("integratorId")]
        public string IntegratorId { get; set; } = null!;
        
        [BsonElement("buildingId")]
        public string BuildingId { get; set; } = null!;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [BsonElement("lastUpdatedBy")]
        public string LastUpdatedBy { get; set; } = null!;

        [BsonElement("lastUpdatedAt")]
        public DateTimeOffset LastUpdatedAt { get; set; }
        
        public void CreateChangesTime(Line line)
        {
            line.CreatedAt = DateTimeOffset.UtcNow;
            line.LastUpdatedAt = DateTimeOffset.UtcNow;
            line.CreatedBy = "Creator";
        }

        public void UpdateChangesTime(Line line)
        {
            line.LastUpdatedAt = DateTimeOffset.UtcNow;
            line.LastUpdatedBy = "Updater";
        }
    }
}

