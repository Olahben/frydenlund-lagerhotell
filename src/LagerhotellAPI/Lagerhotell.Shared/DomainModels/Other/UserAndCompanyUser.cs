using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.DomainModels;

public class UserAndCompanyUser
{
    public UserAndCompanyUser(User? user, CompanyUser? companyUser)
    {
        CompanyUser = companyUser;
        User = user;
    }
    public CompanyUser? CompanyUser { get; set; }
    public User? User { get; set; }
}
