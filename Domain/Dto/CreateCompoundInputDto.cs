using GeoJSON.Net.Geometry;

namespace Domain.Dto
{
	public class CreateCompoundInputDto
	{
		public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }
        public Point? Coordinates { get; set; }
        public string ClientId { get; set; }
    }
}

