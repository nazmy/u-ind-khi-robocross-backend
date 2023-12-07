using System;
using domain.Dto;
using GeoJSON.Net.Geometry;

namespace Domain.Dto
{
	public class UpdateTimelineInput
	{
        public string Id { get; set; }

        public String[]? TimelineDetailsIds  { get; set; }
		
        public CreateTimelineDetailsInput[]? TimelineDetails  { get; set; }
        
        public string UnitId { get; set; }
    }
}

