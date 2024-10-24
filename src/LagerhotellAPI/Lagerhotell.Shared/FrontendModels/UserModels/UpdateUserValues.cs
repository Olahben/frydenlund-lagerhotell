namespace LagerhotellAPI.Models.FrontendModels;

public class UpdateUserValuesRequest2
{
    public required string UserId { get; set; }
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }

    public required string BirthDate { get; set; }
    public required string Password { get; set; }
    public required Address Address { get; set; }
    public required bool IsAdministrator { get; set; }
    public required string Email { get; set; }
    public required bool IsEmailVerified { get; set; }
}

public class UpdateUserValuesRequest
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }

    public required string BirthDate { get; set; }
    public required string Password { get; set; }
    public required Address Address { get; set; }
    public required bool IsAdministrator { get; set; }
    public required string Email { get; set; }
}

public class UpdateUserValuesResponse
{
    public required bool Success { get; set; }
}
