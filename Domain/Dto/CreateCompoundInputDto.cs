using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Dto
{
	public class CreateCompoundInputDto
	{
		public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string PicName { get; set; }

        public string PicContactNumber { get; set; }

        public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; set; }
    }
}

