namespace Domain.Helper;

public class RobocrossBlobStorageSettings : IRobocrossBlobStorageSettings
{
    public string ConnectionString { get; set; }
    public string Key { get; set; }
}