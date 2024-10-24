namespace LagerhotellAPI.Models.FrontendModels;

public class DeleteWarehouseHotelResponse
{
    public DeleteWarehouseHotelResponse(string warehouseHotelId, string name)
    {
        WarehouseHotelId = warehouseHotelId;
        Name = name;
    }
    public string WarehouseHotelId { get; set; }
    public string Name { get; set; }
}
