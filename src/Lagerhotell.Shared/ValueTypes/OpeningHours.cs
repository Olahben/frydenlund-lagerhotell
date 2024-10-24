namespace LagerhotellAPI.Models.ValueTypes;

public class OpeningHours
{
    public double? Opens { get; set; }
    public double? Closes { get; set; }
    public OpeningHours(double opens, double closes)
    {
        if (opens < 0 || closes < 0 || opens > Closes)
        {
            throw new ArgumentException("Feil");
        }
        Opens = opens;
        Closes = closes;
    }

    public OpeningHours() { }
}
