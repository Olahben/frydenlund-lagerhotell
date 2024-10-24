using System.Data.SqlTypes;
namespace LagerhotellAPI.Services;
public interface ICompanyUserService
{

    Task<CompanyUser> GetCompanyUserAsync(string id);
    Task<List<CompanyUser>> GetCompanyUsersAsync(int? take, int? skip);
    Task<CompanyUser> GetCompanyUserByPhoneNumber(string phoneNumber);
    Task<(string, string)> CreateCompanyUserAsync(CompanyUser companyUser);
    Task UpdateCompanyUserAsync(string id, CompanyUser companyUser);
    Task DeleteCompanyUserAsync(string id);
    Task<(string, string)> LoginCompanyUserByEmail(string email, string password);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newPassword"></param>
    /// <param name="oldPassword"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="SqlAlreadyFilledException"></exception>
    /// <exception cref="SqlTypeException"></exception>
    Task ResetPassword(string id, string newPassword, string oldPassword);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="companyNumber"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> DoesSimilarUserExist(string companyNumber, string phoneNumber, string email);
    Task<CompanyUser> GetUserByAuth0Id(string id);

}
