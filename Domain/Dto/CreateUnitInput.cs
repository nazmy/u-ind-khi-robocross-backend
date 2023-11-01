namespace domain.Dto;

public class CreateUnitInput
{
    public string Name { get; set; }
    
    public string[] Plcs { get; set; }
    
    public List<CreateSceneObjectInput> SceneObjects { get; set;}
}