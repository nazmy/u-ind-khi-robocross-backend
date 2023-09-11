namespace domain.Dto;

public class UpdateUnitInputDto
{
    public string Name { get; set; }
    
    public string[] Pics { get; set; }
    
    public UpdateSceneObjectInput[] SceneObject { get; set;}
}