using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace domain.Dto
{
	public class CreateBuildingInputDto : BaseDto
	{
		public string Name { get; set; }

		public string PicName { get; set; }

		public string PicPhoneNumber { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Coordinates { get; set; }

        public string CompoundId { get; set; }
    }
}

