namespace domain.Dto;

public class UpdateAssetInput : BaseDto
{
    public string GlbModelUrl { get; set; }
    public string PngThumbnailUrl { get; set; }
    public string ComponentJson { get; set; }
    public string Tags { get; set; }
    public string SlotsCompatibleLibraryIds{ get; set; }
    public bool? ConstantSlotCount { get; set; }
    public IDictionary<string,string>? Metadata { get; set; }
}