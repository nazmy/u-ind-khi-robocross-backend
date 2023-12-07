using System.Text.Json.Serialization;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
    public class BulkTimelineDetailsResponse
    {
        public string Id { get; set; }
		
        public string? Name { get; set; }
		
        public TimelineTrack[] Track { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedAt { get; set; }

        public string LastUpdatedBy { get; set; }

        public string LastUpdatedAt { get; set; }

    }
}