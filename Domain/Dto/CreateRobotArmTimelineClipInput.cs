using Domain.Entities;
using Domain.Helper;

namespace domain.Dto;

public class CreateRobotArmTimelineClipInput
{
    public string Type { get; } = nameof(RobotArmTimelineClip);
    
    public float StartTime { get; set; }
    
    public float EndTime { get; set; }
    
    public ObjectAction ObjectAction { get; set; }
    
    public string? DestinationObjectId { get; set; }

    public string? HandlingObjectId { get; set; }
}