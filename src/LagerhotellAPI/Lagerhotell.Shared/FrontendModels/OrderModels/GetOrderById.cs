namespace LagerhotellAPI.Models.FrontendModels;

// Use static class on top
public class GetOrderByIdRequest
{
    public required string Id { get; set; }
}

public class GetOrderByIdResponse
{
    // Write direct properties for order return
    public required Order Order { get; set; }
}
