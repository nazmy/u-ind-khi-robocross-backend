namespace domain.Dto;

public class AssetResponse : BaseDto
{
    public string Name { get; set; }
    public string GlbModelUrl { get; set; }
    public string PngThumbnailUrl { get; set; }
    public string ComponentJson { get; set; }
    public string Tags { get; set; }
    public string SlotsCompatibleLibraryIds{ get; set; }
    public bool? ConstantSlotCount { get; set; }
    public IDictionary<string,string>? Metadata { get; set; }
}