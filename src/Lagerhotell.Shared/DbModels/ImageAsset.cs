using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

public class ImageAsset
{

    public ImageAsset() { }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? AssetId { get; set; }
    public string Name { get; set; }
    public List<string> Tags { get; set; }
    public byte[] ImageBytes { get; set; }
    public string? WarehouseHotelId { get; set; }
}
