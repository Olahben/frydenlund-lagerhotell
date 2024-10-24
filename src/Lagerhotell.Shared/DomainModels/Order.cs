namespace LagerhotellAPI.Models.DomainModels
{
    public record Order
    {
        public string OrderId { get; init; }
        public string UserId { get; init; }
        public string StorageUnitId { get; init; }
        public OrderPeriod OrderPeriod { get; init; }
        public OrderStatus? Status { get; init; } = OrderStatus.NotCreated;
        public OrderInsurance? Insurance { get; init; } = OrderInsurance.FiftyThousand;
        public string? CustomInstructions { get; init; }

        // Parameterless constructor
        public Order() { }

        // Parameterized constructor
        public Order(string orderId, string userId, string storageUnitId, OrderPeriod orderPeriod, OrderStatus? status, OrderInsurance? insurance, string? customInstructions)
        {
            OrderId = orderId;
            UserId = userId;
            StorageUnitId = storageUnitId;
            OrderPeriod = orderPeriod;
            Status = status;
            Insurance = insurance;
            CustomInstructions = customInstructions;
        }
    }
}
