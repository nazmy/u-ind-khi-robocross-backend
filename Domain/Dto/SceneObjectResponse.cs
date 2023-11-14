using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace domain.Dto;

public class SceneObjectResponse
{
    public string? Id { get; set; }
    
    public string Name { get; set; }
    
    public string LibraryUrl { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public string SceneObjectType { get; set; }
    
    public string? Manufacturer { get; set; }

    public string? ModelNumber { get; set; }

    public string? SerialNumber { get; set; }

    public DateTimeOffset? ManufacturedDate { get; set; }
    
    public DateTimeOffset? InstallationDate { get; set; }
    
    public int? OperatingStatus { get; set; }
    
    public Single? PowerConsumption { get; set; }
    
    public string? TransformState { get; set; }
    
    public string? ComponentOverride { get; set; }
}