namespace LagerhotellAPI.Models.FrontendModels;

public record ResetPasswordRequest(string UserId, string NewPassword, string OldPassword);