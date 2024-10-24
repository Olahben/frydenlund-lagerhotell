namespace LagerhotellAPI.Models.FrontendModels;

public record CancelOrderRequest(string OrderId);

public record CancelOrderResponse(OrderPeriod OrderPeriod);