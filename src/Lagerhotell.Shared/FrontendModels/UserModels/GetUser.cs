namespace LagerhotellAPI.Models.FrontendModels;


public class GetUser
{
    public class GetUserRequest
    {
        public required string UserId { get; set; }
    }

    public class GetUserResponse
    {
        public User? User { get; set; }
    }
}
