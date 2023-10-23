using Point = GeoJSON.Net.Geometry.Point;


namespace domain.Dto
{
	public class TimelineResponse : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Details { get; set; }

        public string UnitId { get; set; }
    }
}

