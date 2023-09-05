namespace Domain.Helper
{
	public class IRobocrossDatabaseSettings
	{
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
        
        public string ClientsCollectionName { get; set; }

        public string CompoundsCollectionName { get; set; }
        
        public string BuildingsCollectionName { get; set; }
    }
}

