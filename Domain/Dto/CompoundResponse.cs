using domain.Dto;
using Point = GeoJSON.Net.Geometry.Point;

namespace Domain.Dto
{
	public class CompoundResponse : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }
        
        public string Address { get; set; }

        public Point Coordinates { get; set; }

        public string ClientId { get; set; }
    }
}


