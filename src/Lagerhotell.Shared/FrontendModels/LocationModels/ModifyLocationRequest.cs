namespace LagerhotellAPI.Models.FrontendModels;

public class ModifyLocationRequest
{
    public ModifyLocationRequest(string oldLocationName, Location location)
    {
        OldLocationName = oldLocationName;
        Location = location;
    }
    public string OldLocationName { get; set; }
    public Location Location { get; set; }
}
