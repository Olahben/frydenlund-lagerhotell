namespace LagerhotellAPI.Models.DomainModels;

// Used to display orders and storage units in the "orders" section of the customer page
public record OrderCustomerViewModel(Order Order, StorageUnit StorageUnit, string WarehouseHotelName);
