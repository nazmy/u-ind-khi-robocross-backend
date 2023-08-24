using System;
namespace domain.Dto
{
	public class BaseDto
	{
        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public string LastUpdatedBy { get; set; }

        public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}

