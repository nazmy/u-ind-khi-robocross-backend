namespace Domain.Helper
{
	public class RobocrossDatabaseSettings : IRobocrossDatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ClientsCollectionName { get; set; } = null!;

        public string CompoundsCollectionName { get; set; } = null!;
        
        public string BuildingsCollectionName { get; set; } = null!;
        
        public string LinesCollectionName { get; set; } = null!;
        
        public string TimelinesCollectionName { get; set; } = null!;
        
        public string MessagesCollectionName { get; set; } = null!;
        
        public string UsersCollectionName { get; set; } = null!;
    }
}

