namespace LagerhotellAPI.Models.FrontendModels;


public class GetUserByPhoneNumber
{
    public class GetUserByPhoneNumberRequest
    {
        public required string PhoneNumber { get; set; }
    }
    public class GetUserByPhoneNumberResponse
    {
        public User User { get; set; }
        public string Id { get; set; }
    }

}
