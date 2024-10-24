namespace LagerhotellAPI.Models.DomainModels;

public class ImageAsset
{
    public ImageAsset()
    {
        // empty constructor
    }

    public string? AssetId { get; set; }
    public string Name { get; set; }
    public List<string>? Tags { get; set; }
    public byte[] ImageBytes { get; set; }
    public string? WarehouseHotelId { get; set; }
}
