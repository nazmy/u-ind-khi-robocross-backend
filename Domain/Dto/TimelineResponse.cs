using Point = GeoJSON.Net.Geometry.Point;


namespace domain.Dto
{
	public class TimelineResponse : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public String[]? TimelineDetailsIds  { get; set; }
        
        public TimelineDetailsResponse[]? TimelineDetails  { get; set; }
        
        public string UnitId { get; set; }
    }
}

