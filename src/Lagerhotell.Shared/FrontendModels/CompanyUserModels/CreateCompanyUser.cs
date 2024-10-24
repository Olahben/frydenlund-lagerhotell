namespace LagerhotellAPI.Models.FrontendModels;

public record CreateCompanyUserRequest(CompanyUser CompanyUser);
public record CreateCompanyUserResponse(string CompanyUserId, string UserAcessToken);

