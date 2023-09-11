namespace domain.Dto;

public class SceneObjectResponse : BaseDto
{
    public string LibaryUrl { get; set; }
    
    public List<RobotResponse> Robots { get; set; }
}