namespace domain.Dto;

public class UpdateUnitInput
{
    public string? Id { get; set; }
    
    public string Name { get; set; }
    
    public string[] Plcs { get; set; }
    
    public List<UpdateSceneObjectInput> SceneObjects { get; set;}
}