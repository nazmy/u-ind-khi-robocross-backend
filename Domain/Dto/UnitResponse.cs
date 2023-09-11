namespace domain.Dto;

public class UnitResponse
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string[] Pics { get; set; }
    
    public List<SceneObjectResponse> SceneObjects { get; set;}
}