using System;
using domain.Dto;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Dto
{
	public class CompoundOutputDto : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }

        public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; set; }

    }
}


