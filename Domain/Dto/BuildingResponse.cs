using Point = GeoJSON.Net.Geometry.Point;


namespace domain.Dto
{
	public class BuildingResponse : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }

        public Point? Coordinates { get; set; }

        public string CompoundId { get; set; }
    }
}

