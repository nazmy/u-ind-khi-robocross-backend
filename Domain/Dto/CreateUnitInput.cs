namespace domain.Dto;

public class CreateUnitInput
{
    public string Name { get; set; }
    
    public string[] Pics { get; set; }
    
    public List<CreateSceneObjectInput> SceneObjects { get; set;}
}