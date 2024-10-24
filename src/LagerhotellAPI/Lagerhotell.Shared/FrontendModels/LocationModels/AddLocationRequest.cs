namespace LagerhotellAPI.Models.FrontendModels;

public class AddLocationRequest
{
    public AddLocationRequest(Location location)
    {
        Location = location;
    }
    public Location Location { get; set; }
}
