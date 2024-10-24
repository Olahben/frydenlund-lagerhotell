namespace LagerhotellAPI.Models.ValueTypes;

public class Dimensions
{
    public double? Length { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public string? InfoText { get; set; }

    public Dimensions(double length, double width, double height, string? infoText)
    {
        if (length < 0 || width < 0 || height < 0)
            throw new ArgumentException("Dimensions must be non-negative.");

        Length = length;
        Width = width;
        Height = height;
        InfoText = infoText;
    }

    public Dimensions()
    {

    }

    public double? Area => Length * Width;
    public double? Volume => Length * Width * Height;
}


