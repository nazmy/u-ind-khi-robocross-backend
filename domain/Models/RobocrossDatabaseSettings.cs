namespace Domain.Models
{
	public class RobocrossDatabaseSettings : IRobocrossDatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ClientsCollectionName { get; set; } = null!;
    }
}

