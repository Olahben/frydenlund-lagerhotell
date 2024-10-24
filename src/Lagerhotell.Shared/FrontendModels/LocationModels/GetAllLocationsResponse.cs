namespace LagerhotellAPI.Models.FrontendModels;

public class GetAllLocationsResponse
{
    public GetAllLocationsResponse(List<Location> locations)
    {
        Locations = locations;
    }
    public List<Location> Locations { get; set; }
}
