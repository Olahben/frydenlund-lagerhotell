namespace LagerhotellAPI.Models.FrontendModels;

public class ModifyWarehouseHotelRequest
{
    public ModifyWarehouseHotelRequest(WarehouseHotel warehouseHotel, string oldWarehouseHotelName, List<ImageAsset> images)
    {
        WarehouseHotel = warehouseHotel;
        OldWarehouseHotelName = oldWarehouseHotelName;
        Images = images;
    }
    public WarehouseHotel WarehouseHotel { get; set; }
    public string OldWarehouseHotelName { get; set; }
    public List<ImageAsset> Images { get; set; }
}
