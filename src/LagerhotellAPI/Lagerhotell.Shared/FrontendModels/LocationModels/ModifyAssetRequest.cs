namespace LagerhotellAPI.Models.FrontendModels;

public class ModifyAssetRequest
{
    public required string AssetId { get; set; }
    public required ImageAsset Asset { get; set; }
}
