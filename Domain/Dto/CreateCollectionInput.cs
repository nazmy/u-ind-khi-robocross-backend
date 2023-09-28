namespace domain.Dto;

public class CreateCollectionInput
{
    public string Name { get; set; }
    public IDictionary<string,string> Metadata { get; set; }
}