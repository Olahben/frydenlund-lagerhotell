namespace LagerhotellAPI.Models.FrontendModels;

public class DeleteOrderRequest
{
    public required string OrderId { get; set; }
}
public class DeleteOrderResponse
{
    public required bool Success { get; set; }
}

