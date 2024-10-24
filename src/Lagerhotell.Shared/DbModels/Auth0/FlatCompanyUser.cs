using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.DbModels.Auth0;

public record FlatCompanyUser(string CompanyUserId, string FirstName, string LastName, string Name, string CompanyNumber, string Email, string PhoneNumber, string Password, string StreetAddress, string PostalCode, string City);