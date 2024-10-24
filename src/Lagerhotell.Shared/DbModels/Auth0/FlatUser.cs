using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.DbModels.Auth0;

public record FlatUser(string UserId, string FirstName, string LastName, string PhoneNumber, string BirthDate, string StreetAddress, string postalCode, string City, string Password, bool IsAdministrator, string Email)
{
    // Parameterless constructor
    public FlatUser() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty)
    {
    }
}
