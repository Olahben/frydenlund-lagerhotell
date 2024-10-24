namespace LagerhotellAPI.Models.FrontendModels;


public class DecodeJwt
{
    public class DecodeJwtResponse
    {
        public string? PhoneNumber { get; set; }
    }
    public class DecodeJwtRequest
    {
        public required string Token { get; set; }
    }
}
