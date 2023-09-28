namespace domain.Dto;

public class UpdateAssetInput
{
    public string? Name { get; set; }
    
    public IDictionary<string,string> Metadata { get; set; }

    public string CollectionName { get; set; }
}