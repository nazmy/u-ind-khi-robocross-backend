using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace domain.Dto
{
	public class BuildingOutputDto : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }

        public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; set; }

        public string CompoundId { get; set; }
    }
}

