namespace LagerhotellAPI.Models.FrontendModels;

public class GetAllOrdersRequest
{

}

public class GetAllOrdersResponse
{
    public required List<Order> Orders { get; set; }
}
