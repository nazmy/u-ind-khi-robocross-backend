
using System;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Entities
{
	public class Line : Base
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
        
        [BsonElement("thumbnailUrl")]
        public string ThumbnailUrl { get; set; } = null!;

        [BsonElement("scannedSceneGLBUrl")]
        public string ScannedSceneGLBUrl { get; set; } = null!;
	
        [BsonElement("status")]
        public LineStatusEnum Status { get; set; }

        [BsonElement("units")] 
        public List<Unit> Units { get; set; } = new List<Unit>();

        [BsonElement("integratorId")]
        public string? IntegratorId { get; set; } = null!;
        
        [BsonElement("buildingId")]
        public string BuildingId { get; set; } = null!;
        
        [BsonElement("clientId")]
        public string ClientId { get; set; } = null!;
    }
}

