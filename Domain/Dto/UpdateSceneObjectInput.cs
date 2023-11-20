using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;

namespace domain.Dto;

public class UpdateSceneObjectInput
{
    public string? Id { get; set; }
    
    public string Name { get; set; }
    
    public string LibraryUrl { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SceneObjectTypeEnum SceneObjectType { get; set; }
    
    public string? Manufacturer { get; set; }

    public string? ModelNumber { get; set; }

    public string? SerialNumber { get; set; }
    
    public DateTimeOffset? LastMaintenanceDate { get; set; }
    
    public DateTimeOffset? NextMaintenanceDate { get; set; }

    public DateTimeOffset? ManufacturedDate { get; set; }
    
    public DateTimeOffset? InstallationDate { get; set; }
    
    public long? OperatingHours { get; set; }
    
    public int? OperatingStatus { get; set; }
    
    public Single? PowerConsumption { get; set; }
    
    public TransformState? TransformState { get; set; }
    
    public string? ComponentOverride { get; set; }
}