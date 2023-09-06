using System;
using System.Text.Json.Serialization;
using Domain.Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
	public class CreateClientInputDto
	{
		public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ClientTypeEnum ClientType { get; set; }
    }
}

