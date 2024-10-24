using LagerhotellAPI.Models.DbModels.Auth0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.FrontendModels;

public record SignupUserAuth0Request(UserAuth0 user);
