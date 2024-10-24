namespace LagerhotellAPI.Models.FrontendModels;

public class AddWarehouseHotelResponse
{
    public AddWarehouseHotelResponse(string warehouseHotelId)
    {
        WarehouseHotelId = warehouseHotelId;
    }
    public string WarehouseHotelId { get; set; }
}
