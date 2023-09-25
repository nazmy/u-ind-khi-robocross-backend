
using Domain.Helper;

namespace Domain.Dto
{
	public class UpdateClientInput
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
        
        public ClientTypeEnum ClientType { get; set; }
    }
}

