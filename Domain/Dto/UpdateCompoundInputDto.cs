using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Domain.Dto
{
	public class UpdateCompoundInputDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string PicName { get; set; }

        public string PicPhoneNumber { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Coordinates { get; set; }
    }
}

