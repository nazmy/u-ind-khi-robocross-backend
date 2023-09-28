namespace domain.Dto;

public class CollectionResponse
{
    public string Name;
    public IDictionary<string, string> Metadata;
    public DateTimeOffset LastModified;
}