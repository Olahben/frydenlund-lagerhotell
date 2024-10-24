namespace LagerhotellAPI.Models.FrontendModels;

public class AddOrderRequest
{
    public required Order Order { get; set; }
}
public class AddOrderResponse
{
    public AddOrderResponse(string orderId)
    {
        OrderId = orderId;
    }
    public string OrderId { get; set; }

}
