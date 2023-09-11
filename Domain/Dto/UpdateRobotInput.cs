namespace domain.Dto;

public class UpdateRobotInput
{
    public string? Id { get; set; }
    
    public string Manufacturer { get; set; }

    public string ModelNumber { get; set; }

    public string SerialNumber { get; set; }

    public DateTimeOffset ManufacturedDate { get; set; }
    
    public DateTimeOffset InstallationDate { get; set; }
    
    public int OperatingStatus { get; set; }
    
    public Single PowerConsumption { get; set; }
}