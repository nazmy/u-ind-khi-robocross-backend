using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Robot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("manufacturer")]
    public string Manufacturer { get; set; } = null!;
    
    [BsonElement("modelNumber")]
    public string ModelNumber { get; set; } = null!;
    
    [BsonElement("serialNumber")]
    public string SerialNumber { get; set; } = null!;
    
    [BsonElement("manufacturedDate")]
    public DateTimeOffset ManufacturedDate { get; set; }
    
    [BsonElement("installationDate")]
    public DateTimeOffset InstallationDate { get; set; }
    
    [BsonElement("operatingStatus")]
    public int OperatingStatus { get; set; }
    
    [BsonElement("powerConsumption")]
    public Single PowerConsumption { get; set; }
}