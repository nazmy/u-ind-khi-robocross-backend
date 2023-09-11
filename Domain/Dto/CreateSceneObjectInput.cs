namespace domain.Dto;

public class CreateSceneObjectInput
{
    public string LibraryUrl { get; set; }
    
    public List<CreateRobotInput> Robots { get; set; }
}