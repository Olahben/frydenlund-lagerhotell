namespace LagerhotellAPI.Models.DomainModels;

public class Location
{
    public Location(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }

    public Location()
    {

    }

    public string Name { get; set; }
    public bool IsActive { get; set; }
}
