namespace Domain.Helper
{
	public class RobocrossDatabaseSettings : IRobocrossDatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ClientsCollectionName { get; set; } = null!;

        public string CompoundsCollectionName { get; set; } = null!;
    }
}

