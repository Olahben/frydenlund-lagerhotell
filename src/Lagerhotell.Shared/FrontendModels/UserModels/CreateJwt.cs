namespace LagerhotellAPI.Models.FrontendModels;


public class CreateJwt
{
    public class CreateJwtRequestService
    {
        public required string PhoneNumber { get; set; }
    }

    public class CreateJwtRequest
    {
        public required string Id { get; set; }
    }

    public class CreateJwtResponse
    {
        public string? JWT { get; set; }
    }
}
