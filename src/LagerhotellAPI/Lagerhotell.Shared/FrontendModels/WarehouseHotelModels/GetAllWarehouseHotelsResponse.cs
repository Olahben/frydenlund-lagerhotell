namespace LagerhotellAPI.Models.FrontendModels;

public class GetAllWarehouseHotelsResponse
{
    public GetAllWarehouseHotelsResponse(List<WarehouseHotel> warehouseHotels)
    {
        WarehouseHotels = warehouseHotels;
    }
    public List<WarehouseHotel> WarehouseHotels { get; set; }
}
