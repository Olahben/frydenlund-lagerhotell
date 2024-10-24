namespace LagerhotellAPI.Models.ValueTypes;

public class Money
{
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }

    public Money(decimal amount, string currency = "NOK")
    {
        Amount = amount;
        Currency = currency;
    }

    public Money()
    {

    }
}

