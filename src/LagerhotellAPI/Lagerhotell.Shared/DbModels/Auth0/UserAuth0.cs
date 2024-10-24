using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.DbModels.Auth0;

public record UserAuth0(string UserId, string Email, bool EmailVerified)
{
    public string? UserAuth0Id { get; set; }
    public string? Password { get; set; }
    // Empty constructor
    public UserAuth0() : this(string.Empty, string.Empty, false) { }
};