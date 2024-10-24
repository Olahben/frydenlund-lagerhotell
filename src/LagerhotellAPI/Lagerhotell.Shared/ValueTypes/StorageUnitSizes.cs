namespace LagerhotellAPI.Models.ValueTypes;

public class StorageUnitSizes
{
    public double? MinSize { get; set; }
    public double? MaxSize { get; set; } = 0;

    public StorageUnitSizes(double minSize, double maxSize)
    {
        if (MinSize < 0 || MaxSize < 0 || MinSize > MaxSize)
        {
            throw new ArgumentException("Feil");
        }
        MinSize = minSize;
        MaxSize = maxSize;
    }

    public StorageUnitSizes() { }
}
