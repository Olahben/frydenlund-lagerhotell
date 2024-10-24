namespace LagerhotellAPI.Services
{
    public class PendingOrderHandler : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PendingOrderHandler> _logger;
        private Timer _timer;
        private OrderService _orderService;

        public PendingOrderHandler(IServiceScopeFactory serviceScopeFactory, ILogger<PendingOrderHandler> logger, OrderService orderService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _orderService = orderService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PendingOrderHandler is starting.");
            _timer = new Timer(RetrieveActivePendingOrders, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void RetrieveActivePendingOrders(object state)
        {
            _logger.LogInformation("PendingOrderHandler is executing retreiving all pending active orders.");

            List<Order> pendingOrders = await _orderService.GetAllOrders(userId: null, skip: 0, take: 0, orderStatus: OrderStatus.Pending);

            if (pendingOrders.Count == 0)
            {
                _logger.LogInformation("No pending orders found.");
            }
            else
            {
                foreach (var order in pendingOrders)
                {
                    _logger.LogInformation($"Processing order ID: {order.OrderId}, Status: {order.Status}");
                    // Send "e-faktura" on email to the user
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PendingOrderHandler is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
