using LagerhotellAPI.Models;
using LagerhotellAPI.Models.DbModels;

namespace LagerhotellAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Models.DbModels.OrderDocument> _orders;
        private readonly StorageUnitService _storageUnitService;
        public OrderService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _orders = database.GetCollection<Models.DbModels.OrderDocument>("Orders");
            _storageUnitService = new StorageUnitService(settings);
        }

        /// <summary>
        /// Adds a order to the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Id</returns>
        public async Task<string> AddOrder(Order order)
        {
            string orderId = Guid.NewGuid().ToString();
            // Id is set automatically by mongoDB
            order.OrderPeriod.NextPaymentDate = order.OrderPeriod.OrderDate ?? DateTime.MinValue;
            LagerhotellAPI.Models.DbModels.OrderDocument dbOrder = new(orderId, order.UserId, order.StorageUnitId, order.OrderPeriod, order.Status, order.Insurance, order.CustomInstructions);
            await _orders.InsertOneAsync(dbOrder);
            return orderId;
        }

        /// <summary>
        /// Deletes a order from the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task DeleteOrder(string orderId)
        {
            await _orders.DeleteOneAsync(order => order.Id == orderId);
        }

        /// <summary>
        /// Modifies an order in the database (does not modify the orderId)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="updatedOrder"></param>
        /// <returns></returns>
        public async Task ModifyOrder(string orderId, Order updatedOrder)
        {
            OrderDocument orderDocument = await GetOrderDbModel(orderId);
            LagerhotellAPI.Models.DbModels.OrderDocument updatedDbOrder = new(orderId, updatedOrder.UserId, updatedOrder.StorageUnitId, updatedOrder.OrderPeriod, updatedOrder.Status, updatedOrder.Insurance, updatedOrder.CustomInstructions) { Id = orderDocument.Id };
            await _orders.ReplaceOneAsync(order => order.OrderId == orderId, updatedDbOrder);
        }

        /// <summary>
        /// Gets an order from the database with the given order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>order</returns>
        public async Task<Order?> GetOrder(string orderId)
        {
            var dbOrder = await _orders.Find(order => order.OrderId == orderId).FirstOrDefaultAsync();
            if (dbOrder == null)
            {
                return null;
            }
            LagerhotellAPI.Models.DomainModels.Order domainOrder = new(dbOrder.OrderId, dbOrder.UserId, dbOrder.StorageUnitId, dbOrder.OrderPeriod, dbOrder.Status, dbOrder.Insurance, dbOrder.CustomInstructions);
            return domainOrder;
        }

        /// <summary>
        /// Gets the order with the given order Id from the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>database model of order</returns>
        public async Task<Models.DbModels.OrderDocument> GetOrderDbModel(string orderId)
        {
            var dbOrder = await _orders.Find(order => order.OrderId == orderId).FirstOrDefaultAsync();
            return dbOrder;
        }

        /// <summary>
        /// Gets all orders from the database optionally filtered by userId and orderStatus.
        /// </summary>
        /// <param name="userId">The user ID to filter orders by (optional).</param>
        /// <param name="skip">Number of records to skip (optional).</param>
        /// <param name="take">Number of records to take (optional).</param>
        /// <param name="orderStatus">The order status to filter by (optional).</param>
        /// <returns>A list of orders filtered by the specified criteria.</returns>
        public async Task<List<Models.DomainModels.Order>> GetAllOrders(string? userId, int? skip, int? take, OrderStatus? orderStatus)
        {
            var filterBuilder = Builders<Models.DbModels.OrderDocument>.Filter;
            var filter = filterBuilder.Empty; // Default filter

            if (userId != null)
            {
                filter = filterBuilder.Eq(order => order.UserId, userId);
            }

            if (orderStatus != null)
            {
                filter &= filterBuilder.Eq(order => order.Status, orderStatus);
            }

            List<Models.DbModels.OrderDocument> dbOrders = await _orders.Find(filter)
                                                       .Skip(skip ?? 0)
                                                       .Limit(take ?? int.MaxValue)
                                                       .ToListAsync();

            List<Models.DomainModels.Order> domainOrders = dbOrders.ConvertAll(dbOrder =>
                new Models.DomainModels.Order(dbOrder.OrderId, dbOrder.UserId, dbOrder.StorageUnitId, dbOrder.OrderPeriod, dbOrder.Status, dbOrder.Insurance, dbOrder.CustomInstructions));

            return domainOrders;
        }

        public async Task CancelOrder(string orderId)
        {
            OrderStatus orderStatus = OrderStatus.Cancelled;
            var order = await GetOrder(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Order is already cancelled");
            }
            if (order.Status == OrderStatus.Active)
            {
                orderStatus = OrderStatus.NotActiveAnymore;
            }
            var filter = Builders<Models.DbModels.OrderDocument>.Filter.Eq(order => order.OrderId, orderId);
            var update = Builders<Models.DbModels.OrderDocument>.Update.Set(order => order.Status, orderStatus);
            await _orders.UpdateOneAsync(filter, update);
            await _storageUnitService.VacateStorageUnit(order.StorageUnitId);
        }
    }
}