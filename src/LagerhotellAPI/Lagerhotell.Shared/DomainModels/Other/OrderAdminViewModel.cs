namespace LagerhotellAPI.Models.DomainModels;

public record OrderAdminViewModel(Order Order, User User, string WarehouseHotelName, StorageUnit StorageUnit);
