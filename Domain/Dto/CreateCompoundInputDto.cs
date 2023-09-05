using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;


namespace Domain.Dto
{
	public class CreateCompoundInputDto
	{
		public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }

        public string Address { get; set; }
        public Point? Coordinates { get; set; }
        public string ClientId { get; set; }
    }
}

