using System;
using domain.Dto;

namespace Domain.Dto
{
	public class ClientOutputDto : BaseDto
	{
        public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
    }
}


