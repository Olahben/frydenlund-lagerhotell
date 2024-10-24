namespace LagerhotellAPI.Models.DomainModels;

public class StorageUnit
{
    public StorageUnit(string storageUnitId, Dimensions dimensions, bool temperated, string lockCode, string name, bool occupied, string warehouseHotelId, string userId, Coordinate coordinate, Money pricePerMonth)
    {
        StorageUnitId = storageUnitId;
        Dimensions = dimensions;
        Temperated = temperated;
        LockCode = lockCode;
        Name = name;
        Occupied = occupied;
        UserId = userId;
        WarehouseHotelId = warehouseHotelId;
        Coordinate = coordinate;
        PricePerMonth = pricePerMonth;
    }

    public StorageUnit() { }
    public string? StorageUnitId { get; set; }
    public Dimensions Dimensions { get; set; } = new();

    public bool Temperated { get; set; }

    public string? LockCode { get; set; }

    public string Name { get; set; }
    public bool Occupied { get; set; } = false;

    public string? UserId { get; set; }

    public string? WarehouseHotelId { get; set; }

    public Coordinate Coordinate { get; set; } = new();

    public Money PricePerMonth { get; set; } = new();
}
