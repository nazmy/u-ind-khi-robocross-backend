namespace domain.Dto;

public class AssetResponse
{
    public string Name;
    public IDictionary<string, string> Metadata;
    public DateTimeOffset? LastModified { get; set; }
}