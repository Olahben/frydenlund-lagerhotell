namespace LagerhotellAPI.Models.FrontendModels;

public class GetAllOrdersByUserRequest
{
    public required User User { get; set; }
}

public class GetAllOrdersByUserResponse
{
    public List<Order>? Orders { get; set; }
}
