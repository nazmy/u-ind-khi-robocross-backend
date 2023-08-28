using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Dto
{
	public class CreateCompoundInputDto
	{
		public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Coordinates { get; set; }

        public string ClientId { get; set; }
    }
}

