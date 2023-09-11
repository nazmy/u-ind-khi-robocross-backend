namespace domain.Dto;

public class SceneObjectResponse
{
    public string? Id { get; set; }
    
    public string LibraryUrl { get; set; }
    
    public List<RobotResponse> Robots { get; set; }
}