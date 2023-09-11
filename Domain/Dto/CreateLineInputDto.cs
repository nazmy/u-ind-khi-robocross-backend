using System;
using System.Text.Json.Serialization;
using domain.Dto;
using Domain.Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
	public class CreateLineInputDto
	{
		public string Name { get; set; }

        public uint Applications { get; set; }

        public string Description { get; set; }
        
        public string ScannedSceneGLBUrl{ get; set; }
        public int Status { get; set; }
        
        public List<CreateUnitInput> Units { get; set; }
        
        public string IntegratorId { get; set; }

        public string BuildingId { get; set; }
    }
}

