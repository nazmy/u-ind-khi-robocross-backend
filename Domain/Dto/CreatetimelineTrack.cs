namespace domain.Dto;

public class CreatetimelineTrack
{
    public string sceneObjectId { get; set; }
    
    public CreateRobotArmTimelineClipInput[] Clips { get; set; }
}