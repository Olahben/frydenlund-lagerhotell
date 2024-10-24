namespace LagerhotellAPI.Models.FrontendModels;

public record LoginCompanyUserByEmailRequest(string Email, string Password);
public record LoginCompanyUserByEmailResponse(string AccessToken, string UserId);
