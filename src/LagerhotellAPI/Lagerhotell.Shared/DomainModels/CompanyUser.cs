namespace LagerhotellAPI.Models.DomainModels;

public class CompanyUser
{
    public string? CompanyUserId { get; set; }
    public string? Auth0Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string CompanyNumber { get; set; }
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public Address Address { get; set; }

    public CompanyUser(string companyUserId, string firstName, string lastName, string name, string companyNumber, string email, string phoneNumber, Address address, string password, bool isEmailVerified, string auth0Id)
    {
        CompanyUserId = companyUserId;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
        CompanyNumber = companyNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Password = password;
        IsEmailVerified = isEmailVerified;
        Auth0Id = auth0Id;
    }

    public CompanyUser()
    {
        // empty ctor
        Address = new Address();
    }
};
