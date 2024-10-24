namespace LagerhotellAPI.Models.ValueTypes;

public class OrderId
{
    public required string Value { get; set; }

    public OrderId(string value)
    {
        // Validation logic
        Value = value;
    }
}

public class UserId
{
    public required string Value { get; set; }

    public UserId(string value)
    {
        // Validation logic
        Value = value;
    }
}

public class StorageUnitId
{
    public required string Value { get; set; }

    public StorageUnitId(string value)
    {
        // Validation logic
        Value = value;
    }
}

