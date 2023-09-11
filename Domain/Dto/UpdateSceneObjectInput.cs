namespace domain.Dto;

public class UpdateSceneObjectInput
{
    public string LibaryUrl { get; set; }
    
    public UpdateRobotInput[] Robots { get; set; }
}