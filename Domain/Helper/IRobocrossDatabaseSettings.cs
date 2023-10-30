namespace Domain.Helper
{
	public class IRobocrossDatabaseSettings
	{
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
        
        public string ClientsCollectionName { get; set; }

        public string CompoundsCollectionName { get; set; }
        
        public string BuildingsCollectionName { get; set; }
        
        public string LinesCollectionName { get; set; }
        
        public string TimelinesCollectionName { get; set; } = null!;
        
        public string MessagesCollectionName { get; set; } = null!;
        
        public string UsersCollectionName { get; set; } = null!;
    }
}

