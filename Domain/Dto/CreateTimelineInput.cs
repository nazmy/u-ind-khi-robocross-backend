using System;
using GeoJSON.Net.Geometry;
using MongoDB.Driver.GeoJsonObjectModel;

namespace domain.Dto
{
	public class CreateTimelineInput
	{
		public string Name { get; set; }
		
		public String[] Details  { get; set; }
		
        public string UnitId { get; set; }
    }
}

