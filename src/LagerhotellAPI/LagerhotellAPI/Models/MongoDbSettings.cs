namespace LagerhotellAPI.Models
{
    public class MongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? UsersCollectionName { get; set; }
        // Add other MongoDB related settings here if needed
    }
}
