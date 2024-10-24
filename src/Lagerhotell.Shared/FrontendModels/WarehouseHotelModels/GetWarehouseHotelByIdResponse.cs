using System.Text.Json.Serialization;

namespace LagerhotellAPI.Models.FrontendModels;

public class GetWarehouseHotelByIdResponse
{
    public GetWarehouseHotelByIdResponse(WarehouseHotel warehouseHotel)
    {
        WarehouseHotel = warehouseHotel;
    }
    [JsonPropertyName("warehouseHotel")]
    public WarehouseHotel WarehouseHotel { get; set; }
}
