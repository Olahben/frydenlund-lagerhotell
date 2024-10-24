namespace LagerhotellAPI.Models.FrontendModels;

public class AddWarehouseHotelRequest
{
    public WarehouseHotel WarehouseHotel { get; set; }
    public List<ImageAsset> Images { get; set; }
    public AddWarehouseHotelRequest(WarehouseHotel warehouseHotel, List<ImageAsset> images)
    {
        WarehouseHotel = warehouseHotel;
        Images = images;
    }
}
