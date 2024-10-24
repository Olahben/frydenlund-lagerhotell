namespace LagerhotellAPI.Models.FrontendModels;


public class UpdateOrderRequest
{
    public required Order Order { get; set; }
}

public class UpdateOrderResponse
{
    public required bool Success { get; set; }
}
