namespace Domain.Helper;

public interface IRobocrossBlobStorageSettings
{
    public string ConnectionString { get; set; }
    
    public string Key { get; set; }
}