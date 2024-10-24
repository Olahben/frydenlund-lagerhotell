namespace LagerhotellAPI.Models.FrontendModels;


public class CheckPhoneNumber
{
    public class CheckPhoneNumberRequest
    {
        public required string PhoneNumber { get; set; }
    }

    public class CheckPhoneNumberResponse { public bool PhoneNumberExistence { get; set; } }
}
