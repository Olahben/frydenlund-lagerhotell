namespace LagerhotellAPI.Models.ValueTypes;

public class Address
{
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }

    public Address(string? streetAddress, string? postalCode, string? city)
    {
        StreetAddress = streetAddress;
        PostalCode = postalCode;
        City = city;
    }

    public Address() { }
}