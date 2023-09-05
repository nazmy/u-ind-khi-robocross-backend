﻿using System;
using GeoJSON.Net.Geometry;

namespace Domain.Dto
{
	public class UpdateCompoundInputDto
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

