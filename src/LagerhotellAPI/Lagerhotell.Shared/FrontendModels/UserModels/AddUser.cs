namespace LagerhotellAPI.Models.FrontendModels;

public class AddUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string BirthDate { get; set; }

    public required string Address { get; set; }

    public required string PostalCode { get; set; }

    public required string City { get; set; }
    public required string Password { get; set; }
    public required bool IsAdministrator { get; set; } = false;
    public required string Email { get; set; }

}
public class AddUserResponse
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
}
