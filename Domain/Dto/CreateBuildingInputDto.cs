using System;
using GeoJSON.Net.Geometry;
using MongoDB.Driver.GeoJsonObjectModel;

namespace domain.Dto
{
	public class CreateBuildingInputDto
	{
		public string Name { get; set; }

		public string PicName { get; set; }

		public string PicPhoneNumber { get; set; }

        public Point? Coordinates { get; set; }

        public string CompoundId { get; set; }
    }
}

