using Domain.Entities;

namespace domain.Dto;

public class CreateTimelineDetailsInput
{
    public string Name { get; set; }
    
    public CreatetimelineTrack[] Tracks { get; set; }
}