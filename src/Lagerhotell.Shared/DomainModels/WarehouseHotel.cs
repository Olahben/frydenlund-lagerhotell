namespace LagerhotellAPI.Models.DomainModels;

public class WarehouseHotel
{
    public string? WarehouseHotelId { get; set; }
    public Coordinate Coordinate { get; set; } = new();
    public Address Address { get; set; } = new();
    public string Name { get; set; }
    public OpeningHours OpeningHours { get; set; } = new();
    public string PhoneNumber { get; set; }
    public string DetailedDescription { get; set; }

    public string BulletPoints { get; set; }
    public bool ContainsTemperatedStorageUnits { get; set; }
    public bool IsActive { get; set; }
    public StorageUnitSizes StorageUnitsSizes { get; set; } = new();
    public string LocationName { get; set; }

    public WarehouseHotel(Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string detailedDescription, string bulletPoints, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName)
    {
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DetailedDescription = detailedDescription;
        BulletPoints = bulletPoints;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        IsActive = isActive;
        StorageUnitsSizes = storageUnitSizes;
        LocationName = locationName;
    }
    public WarehouseHotel(string warehouseHotelId, Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string detailedDescription, string bulletPoints, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName)
    {
        WarehouseHotelId = warehouseHotelId;
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DetailedDescription = detailedDescription;
        BulletPoints = bulletPoints;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        IsActive = isActive;
        StorageUnitsSizes = storageUnitSizes;
        LocationName = locationName;
    }

    public WarehouseHotel()
    {
        // Parameterless constructor
    }
}
