using System.Text.Json.Serialization;
using domain.Dto;
using Domain.Helper;

namespace Domain.Dto
{
	public class ClientResponse : BaseDto
	{
		public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string ClientType { get; set; }
	}


}


