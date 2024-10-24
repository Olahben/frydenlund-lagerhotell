namespace LagerhotellAPI.Models.DomainModels;
public class User
{
    public User(string userId, string firstName, string lastName, string phoneNumber, string birthDate, Address address, string password, bool isAdministrator, string email, bool isEmailVerified, string auth0Id)
    {
        Id = userId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BirthDate = birthDate;
        Address = address;
        Password = password;
        IsAdministrator = isAdministrator;
        IsEmailVerified = isEmailVerified;
        Auth0Id = auth0Id;
    }
    public User (string firstName, string lastName, string phoneNumber, string email, string birthDate, Address address, string password, bool isAdministrator, bool isEmailVerified, string auth0Id)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BirthDate = birthDate;
        Address = address;
        Password = password;
        IsAdministrator = isAdministrator;
        IsEmailVerified = isEmailVerified;
        Auth0Id = auth0Id;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }
    public string Id { get; set; }
    public string Auth0Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    public string BirthDate { get; set; }

    public Address Address { get; set; }
    public string Password { get; set; }

    public bool IsAdministrator { get; set; }
}
