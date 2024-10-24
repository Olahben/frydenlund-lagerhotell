using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services
{
    public class StorageUnitService
    {
        private readonly IMongoCollection<LagerhotellAPI.Models.DbModels.StorageUnit> _storageUnits;

        public StorageUnitService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _storageUnits = database.GetCollection<LagerhotellAPI.Models.DbModels.StorageUnit>("StorageUnits");
        }

        /// <summary>
        /// Adds a storage unit to the database
        /// </summary>
        /// <param name="storageUnit"></param>
        /// <returns>storage unit Id</returns>
        public async Task<string> AddStorageUnit(StorageUnit storageUnit, string linkedWarehouseHotelId)
        {
            string storageUnitId = Guid.NewGuid().ToString();
            LagerhotellAPI.Models.DbModels.StorageUnit dbStorageUnit = new(storageUnitId, storageUnit.Dimensions, storageUnit.Temperated, storageUnit.LockCode, storageUnit.Name, storageUnit.Occupied, linkedWarehouseHotelId, storageUnit.UserId, storageUnit.Coordinate, storageUnit.PricePerMonth);
            await _storageUnits.InsertOneAsync(dbStorageUnit);
            return storageUnitId;
        }

        /// <summary>
        /// Deletes a storage unit from the database
        /// </summary>
        /// <param name="storageUnitId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task DeleteStorageUnit(string storageUnitId)
        {
            if (await GetStorageUnitById(storageUnitId) != null)
            {
                await _storageUnits.DeleteOneAsync(unit => unit.StorageUnitId == storageUnitId);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Modifies a storage unit in the database (does not modify the storage unit Id)
        /// </summary>
        /// <param name="storageUnitId"></param>
        /// <param name="updatedStorageUnit"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task ModifyStorageUnit(string storageUnitId, StorageUnit updatedStorageUnit)
        {
            Models.DbModels.StorageUnit oldStorageUnit = await GetStorageUnitByIdDbModel(storageUnitId);
            if (oldStorageUnit != null)
            {
                Models.DbModels.StorageUnit updatedDbStorageUnit = new(oldStorageUnit.Id, updatedStorageUnit.StorageUnitId, updatedStorageUnit.Dimensions, updatedStorageUnit.Temperated, updatedStorageUnit.LockCode, updatedStorageUnit.Name, updatedStorageUnit.Occupied, updatedStorageUnit.WarehouseHotelId, updatedStorageUnit.UserId, updatedStorageUnit.Coordinate, updatedStorageUnit.PricePerMonth);
                await _storageUnits.ReplaceOneAsync(unit => unit.StorageUnitId == storageUnitId, updatedDbStorageUnit);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Gets a storage unit from the database with the given storage unit Id
        /// </summary>
        /// <param name="storageUnitId"></param>
        /// <returns>storage unit object</returns>
        public async Task<StorageUnit> GetStorageUnitById(string storageUnitId)
        {
            var dbStorageUnit = await _storageUnits.Find(unit => unit.StorageUnitId == storageUnitId).FirstOrDefaultAsync();
            LagerhotellAPI.Models.DomainModels.StorageUnit domainStorageUnit = new(dbStorageUnit.StorageUnitId, dbStorageUnit.Dimensions, dbStorageUnit.Temperated, dbStorageUnit.LockCode, dbStorageUnit.Name, dbStorageUnit.Occupied, dbStorageUnit.WarehouseHotelId, dbStorageUnit.UserId, dbStorageUnit.Coordinate, dbStorageUnit.PricePerMonth);
            return domainStorageUnit;
        }

        /// <summary>
        /// Gets all storage units in the database with the given warehouse hotel Id
        /// </summary>
        /// <param name="warehouseHotelId"></param>
        /// <returns>A list of storage units</returns>
        public async Task<List<StorageUnit>> GetStorageUnitsByWarehouseHotelId(string warehouseHotelId)
        {
            var dbStorageUnits = await _storageUnits.Find(unit => unit.WarehouseHotelId == warehouseHotelId).ToListAsync();
            if (dbStorageUnits.Count == 0)
            {
                throw new KeyNotFoundException();
            }
            List<LagerhotellAPI.Models.DomainModels.StorageUnit> domainStorageUnits = dbStorageUnits.ConvertAll(dbStorageUnit =>
            {
                return new LagerhotellAPI.Models.DomainModels.StorageUnit(dbStorageUnit.StorageUnitId, dbStorageUnit.Dimensions, dbStorageUnit.Temperated, dbStorageUnit.LockCode, dbStorageUnit.Name, dbStorageUnit.Occupied, dbStorageUnit.WarehouseHotelId, dbStorageUnit.UserId, dbStorageUnit.Coordinate, dbStorageUnit.PricePerMonth);
            });
            return domainStorageUnits;
        }

        /// <summary>
        /// Gets the storage unit with the given storage unit Id from the database
        /// </summary>
        /// <param name="storageUnitId"></param>
        /// <returns>Database storage unit object</returns>
        public async Task<Models.DbModels.StorageUnit> GetStorageUnitByIdDbModel(string storageUnitId)
        {
            var dbStorageUnit = await _storageUnits.Find(unit => unit.StorageUnitId == storageUnitId).FirstOrDefaultAsync();
            return dbStorageUnit;
        }

        /// <summary>
        /// Gets all storage units in the database
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>A list of all storage units</returns>
        public async Task<List<LagerhotellAPI.Models.DomainModels.StorageUnit>> GetAllStorageUnits(int? skip, int? take)
        {
            List<LagerhotellAPI.Models.DbModels.StorageUnit> dbStorageUnits = await _storageUnits.Find(_ => true).Limit(take).Skip(skip).ToListAsync();
            List<LagerhotellAPI.Models.DomainModels.StorageUnit> domainStorageUnits = dbStorageUnits.ConvertAll(dbStorageUnit =>
            {

                return new LagerhotellAPI.Models.DomainModels.StorageUnit(dbStorageUnit.StorageUnitId, dbStorageUnit.Dimensions, dbStorageUnit.Temperated, dbStorageUnit.LockCode, dbStorageUnit.Name, dbStorageUnit.Occupied, dbStorageUnit.WarehouseHotelId, dbStorageUnit.UserId, dbStorageUnit.Coordinate, dbStorageUnit.PricePerMonth);
            });
            return domainStorageUnits;
        }

        public async Task OccupyStorageUnit(string id, string userId)
        {
            var storageUnit = await GetStorageUnitById(id);
            if (storageUnit == null)
            {
                throw new KeyNotFoundException();
            }
            if (storageUnit.Occupied)
            {
                throw new InvalidOperationException("Storage unit is already occupied.");
            }
            var updatedStorageUnit = Builders<LagerhotellAPI.Models.DbModels.StorageUnit>.Update.Set(unit => unit.Occupied, true).Set(unit => unit.UserId, userId);
            _storageUnits.UpdateOne(unit => unit.StorageUnitId == id, updatedStorageUnit);
        }

        public async Task VacateStorageUnit(string id)
        {
            var storageUnit = await GetStorageUnitById(id);
            if (storageUnit == null)
            {
                throw new KeyNotFoundException();
            }
            if (!storageUnit.Occupied)
            {
                throw new InvalidOperationException("Storage unit is already vacant.");
            }
            var updatedStorageUnit = Builders<LagerhotellAPI.Models.DbModels.StorageUnit>.Update.Set(unit => unit.Occupied, false).Set(unit => unit.UserId, null);
            _storageUnits.UpdateOne(unit => unit.StorageUnitId == id, updatedStorageUnit);
        }
    }
}
