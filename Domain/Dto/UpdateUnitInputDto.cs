namespace domain.Dto;

public class UpdateUnitInputDto
{
    public string? Id { get; set; }
    
    public string Name { get; set; }
    
    public string[] Pics { get; set; }
    
    public List<UpdateSceneObjectInput> SceneObjects { get; set;}
}