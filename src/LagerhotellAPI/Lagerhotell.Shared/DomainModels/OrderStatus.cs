namespace LagerhotellAPI.Models.DomainModels;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
    Active,
    NotActiveAnymore,
    PaymentDue,
    Deleted,
    NotCreated
}
