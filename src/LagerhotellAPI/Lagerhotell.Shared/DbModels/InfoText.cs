namespace LagerhotellAPI.Models.DbModels;
using MongoDB.Bson.Serialization.Attributes;

public class InfoText
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public string InfoTextId { get; set; }
    public string Text { get; set; }
    public InfoTextType Type { get; set; }
    public StorageUnitSizesGroup? StorageUnitSizeGroup { get; set; }
}
