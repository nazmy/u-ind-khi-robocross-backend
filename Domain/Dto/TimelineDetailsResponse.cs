using Domain.Entities;

namespace domain.Dto;

public class TimelineDetailsResponse : BaseDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public TimelineTrack[] Tracks { get; set; }
}