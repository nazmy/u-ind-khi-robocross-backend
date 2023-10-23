using System;
using GeoJSON.Net.Geometry;

namespace Domain.Dto
{
	public class UpdateTimelineInput
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Details { get; set; }
        
        public string UnitId { get; set; }
    }
}

