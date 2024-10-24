using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services;

public class WarehouseHotelService
{
    private readonly IMongoCollection<LagerhotellAPI.Models.DbModels.WarehouseHotel> _warehouseHotels;
    public WarehouseHotelService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _warehouseHotels = database.GetCollection<LagerhotellAPI.Models.DbModels.WarehouseHotel>("WarehouseHotels");
    }

    /// <summary>
    /// Adds a warehouse hotel to the database and assigns the Id
    /// </summary>
    /// <param name="warehouseHotel"></param>
    /// <returns>Id</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> AddWarehouseHotel(WarehouseHotel warehouseHotel)
    {
        try
        {
            if (await GetWarehouseHotelByName(warehouseHotel.Name) != null)
            {
                throw new InvalidOperationException("Warehouse hotel already exists");
            }
        }
        catch (KeyNotFoundException)
        {
            // Continue
        }
        string id = Guid.NewGuid().ToString();
        // Should check if there exists one with the same name
        Models.DbModels.WarehouseHotel dbWarehouseHotel = new(id, warehouseHotel.Coordinate, warehouseHotel.Address, warehouseHotel.Name, warehouseHotel.OpeningHours, warehouseHotel.PhoneNumber, warehouseHotel.DetailedDescription, warehouseHotel.BulletPoints, warehouseHotel.ContainsTemperatedStorageUnits, warehouseHotel.IsActive, warehouseHotel.StorageUnitsSizes, warehouseHotel.LocationName);
        await _warehouseHotels.InsertOneAsync(dbWarehouseHotel);
        return id;
    }

    /// <summary>
    /// Deletes a warehouse hotel from the database with the given Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Id, name</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<(string, string)> DeleteWarehouseHotel(string id)
    {
        WarehouseHotel deletedWarehouseHotel = await GetWarehouseHotelById(id) ?? throw new KeyNotFoundException();
        await _warehouseHotels.DeleteOneAsync(hotel => hotel.WarehouseHotelId == id);
        return (deletedWarehouseHotel.WarehouseHotelId, deletedWarehouseHotel.Name);
    }

    /// <summary>
    /// Modifies a warehouse hotel with the given Id (Does not modify the Id)
    /// </summary>
    /// <param name="warehouseHotel"></param>
    /// <param name="oldName"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<string>? ModifyWarehouseHotel(WarehouseHotel warehouseHotel, string oldName)
    {
        Models.DbModels.WarehouseHotel oldWarehouseHotel = await GetWarehouseHotelByIdDbModel(warehouseHotel.WarehouseHotelId);
        if (oldWarehouseHotel != null)
        {
            LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = new(oldWarehouseHotel.Id, warehouseHotel.WarehouseHotelId, warehouseHotel.Coordinate, warehouseHotel.Address, warehouseHotel.Name, warehouseHotel.OpeningHours, warehouseHotel.PhoneNumber, warehouseHotel.DetailedDescription, warehouseHotel.BulletPoints, warehouseHotel.ContainsTemperatedStorageUnits, warehouseHotel.IsActive, warehouseHotel.StorageUnitsSizes, warehouseHotel.LocationName);
            await _warehouseHotels.ReplaceOneAsync(hotel => hotel.Name == oldName, dbWarehouseHotel);
            return dbWarehouseHotel.WarehouseHotelId;
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    /// <summary>
    /// Gets a warehouse hotel by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>warehouse hotel object</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<WarehouseHotel> GetWarehouseHotelById(string id)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.WarehouseHotelId == id).FirstOrDefaultAsync();
        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        WarehouseHotel domainWarehouseHotel = new(dbWarehouseHotel.WarehouseHotelId, dbWarehouseHotel.Coordinate, dbWarehouseHotel.Address, dbWarehouseHotel.Name, dbWarehouseHotel.OpeningHours, dbWarehouseHotel.PhoneNumber, dbWarehouseHotel.DetailedDescription, dbWarehouseHotel.BulletPoints, dbWarehouseHotel.ContainsTemperatedStorageUnits, dbWarehouseHotel.IsActive, dbWarehouseHotel.StorageUnitsSizes, dbWarehouseHotel.LocationName);
        return domainWarehouseHotel;
    }

    /// <summary>
    /// Gets a warehouse hotel by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>warehouse hotel object</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<WarehouseHotel> GetWarehouseHotelByName(string name)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.Name == name).FirstOrDefaultAsync();

        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        WarehouseHotel domainWarehouseHotel = new(dbWarehouseHotel.WarehouseHotelId, dbWarehouseHotel.Coordinate, dbWarehouseHotel.Address, dbWarehouseHotel.Name, dbWarehouseHotel.OpeningHours, dbWarehouseHotel.PhoneNumber, dbWarehouseHotel.DetailedDescription, dbWarehouseHotel.BulletPoints, dbWarehouseHotel.ContainsTemperatedStorageUnits, dbWarehouseHotel.IsActive, dbWarehouseHotel.StorageUnitsSizes, dbWarehouseHotel.LocationName);
        return domainWarehouseHotel;
    }

    /// <summary>
    /// Gets a warehouse hotel by Id from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The database version of the warehouse hotel object</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Models.DbModels.WarehouseHotel> GetWarehouseHotelByIdDbModel(string id)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.WarehouseHotelId == id).FirstOrDefaultAsync();
        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        return dbWarehouseHotel;
    }

    /// <summary>
    /// Gets all warehouse hotels from the database
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns>A list of all the warehouse hotels</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<List<WarehouseHotel>> GetAllWarehouseHotels(int? skip = 0, int? take = 0)
    {
        List<Models.DbModels.WarehouseHotel> dbWarehouseHotels = await _warehouseHotels.Find(hotel => true).Skip(skip).Limit(take).ToListAsync();
        List<WarehouseHotel> domainWarehouseHotels = dbWarehouseHotels.ConvertAll(hotel =>
        {
            if (hotel == null || hotel.WarehouseHotelId == null)
            {
                throw new KeyNotFoundException();
            }
            return new WarehouseHotel(hotel.WarehouseHotelId, hotel.Coordinate, hotel.Address, hotel.Name, hotel.OpeningHours, hotel.PhoneNumber, hotel.DetailedDescription, hotel.BulletPoints, hotel.ContainsTemperatedStorageUnits, hotel.IsActive, hotel.StorageUnitsSizes, hotel.LocationName);
        });
        return domainWarehouseHotels;
    }
}
