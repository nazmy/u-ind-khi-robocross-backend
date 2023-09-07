using System;
using System.Text.Json.Serialization;
using domain.Dto;
using Domain.Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
	public class LineResponse : BaseDto
	{
		public string Id { get; set; }
		
		public string Name { get; set; }

        public uint Applications { get; set; }

        public string Description { get; set; }
        
        public string ScannedSceneGLBUrl{ get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LineStatusEnum Status { get; set; }

        public string IntegratorId { get; set; }

        public string BuildingId { get; set; }
    }
}

