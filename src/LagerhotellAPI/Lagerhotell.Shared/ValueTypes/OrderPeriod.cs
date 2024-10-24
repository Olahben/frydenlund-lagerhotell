namespace LagerhotellAPI.Models.ValueTypes;

public class OrderPeriod
{
    public required DateTime? OrderDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime TimeCreated { get; set; } = DateTime.Now;

    // Is set when order is created
    public DateTime NextPaymentDate { get; set; }

    public OrderPeriod(DateTime orderDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && endDate < orderDate)
        {
            throw new ArgumentException("EndDate must be after OrderDate");
        }

        OrderDate = orderDate;
        EndDate = endDate;
    }

    public OrderPeriod() { }
}
