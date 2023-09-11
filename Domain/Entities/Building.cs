﻿
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Entities
{
	public class Building
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

        [BsonElement("compoundId")]
        public string compoundId { get; set; }

        public void CreateChangesTime(Building building)
        {
            building.CreatedAt = DateTimeOffset.UtcNow;
            building.CreatedBy = "Creator";
        }

        public void UpdateChangesTime(Building building)
        {
            building.LastUpdatedAt = DateTimeOffset.UtcNow;
            building.LastUpdatedBy = "Updater";
        }
    }
}

