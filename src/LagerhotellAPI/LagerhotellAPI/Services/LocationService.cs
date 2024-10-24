using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services;
public class LocationService
{
    private readonly IMongoCollection<Models.DbModels.Location> _locations;
    public LocationService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _locations = database.GetCollection<Models.DbModels.Location>("Locations");
    }

    /// <summary>
    /// Adds a location to the database
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task Add(Location location)
    {
        try
        {
            if (await Get(location.Name) != null)
            {
                throw new InvalidOperationException("Location already exists");
            }
            Models.DbModels.Location dbLocation = new(location.Name, location.IsActive);
            await _locations.InsertOneAsync(dbLocation);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Location already exists, error in LocationService.Add:", ex);
            throw new InvalidOperationException("Could not add a location with the location name" + location.Name);
        }
    }

    /// <summary>
    /// Deletes a location from the database
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task Delete(string name)
    {
        try
        {

            if (await Get(name) != null)
            {
                await _locations.DeleteOneAsync(location => location.Name == name);
            }
            else
            {
                throw new InvalidOperationException("Could not delete the location with the name" + name);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error in LocationService.Delete, could not delete the location with the name" + name, ex);
            throw new InvalidOperationException($"Could not delete location {name}");
        }
    }

    /// <summary>
    /// Modifies a location in the database
    /// </summary>
    /// <param name="location"></param>
    /// <param name="oldLocationName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task Modify(Location location, string oldLocationName)
    {
        try
        {
            Models.DbModels.Location oldLocation = await GetDbModel(oldLocationName);
            if (oldLocation != null)
            {
                Models.DbModels.Location updatedLocation = new(oldLocation.Id, location.Name, location.IsActive);
                await _locations.ReplaceOneAsync(location => location.Name == oldLocationName, updatedLocation);
            }
            else
            {
                throw new InvalidOperationException("Could not find the location with the name" + location.Name);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error in LocationService.Modify", ex);
            throw new InvalidOperationException($"Error in LocationService.Modify, could not modify the location with the name {oldLocationName}");
        }
    }

    /// <summary>
    /// Gets a location from the database with the given name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Location</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Location?> Get(string name)
    {
        try
        {
            Models.DbModels.Location dbLocation = await _locations.Find(location => location.Name == name).FirstOrDefaultAsync();
            if (dbLocation == null)
            {
                return null;
            }
            Location domainLocation = new(dbLocation.Name, dbLocation.IsActive);
            return domainLocation;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in LocationService.Get", ex);
            throw new KeyNotFoundException("No location found with the name" + name);
        }
    }

    /// <summary>
    /// Gets a location from the database with the given name, and returns the database model version
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Database model location</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Models.DbModels.Location?> GetDbModel(string name)
    {
        try
        {
            Models.DbModels.Location dbLocation = await _locations.Find(location => location.Name == location.Name).FirstOrDefaultAsync();
            if (dbLocation == null)
            {
                return null;
            }
            return dbLocation;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in LocationService.Get", ex);
            throw new KeyNotFoundException("No location found with the name" + name);
        }
    }

    /// <summary>
    /// Gets all the locations from the database
    /// </summary>
    /// <param name="includeNonActive"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns>A list of all the locations</returns>
    public async Task<List<Location>>? GetAll(bool includeNonActive, int? skip, int? take)
    {
        var builder = Builders<LagerhotellAPI.Models.DbModels.Location>.Filter;
        var filter = builder.Empty;

        if (!includeNonActive)
        {
            filter = builder.Eq(Location => Location.IsActive, true);
        }

        List<Models.DbModels.Location> dbLocations = await _locations.Find(filter).Skip(skip).Limit(take).ToListAsync();
        List<Location> domainLocations = dbLocations.ConvertAll(location =>
        {
            return new Location(location.Name, location.IsActive);
        });
        return domainLocations;
    }
}
