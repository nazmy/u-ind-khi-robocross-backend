namespace domain.Dto;

public class UpdateSceneObjectInput
{
    public string? Id { get; set; }
    
    public string LibraryUrl { get; set; }
    
    public List<UpdateRobotInput> Robots { get; set; }
}