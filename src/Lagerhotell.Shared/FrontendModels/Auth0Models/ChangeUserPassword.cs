using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.FrontendModels;

public record ChangeUserPasswordRequest(string Auth0Id, string Email, string OldPassword, string NewPassword);