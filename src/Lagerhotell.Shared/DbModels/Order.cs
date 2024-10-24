global using LagerhotellAPI.Models.DomainModels;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;
public record OrderDocument(string OrderId, string UserId, string StorageUnitId, OrderPeriod OrderPeriod, OrderStatus? Status, OrderInsurance? Insurance, string? CustomInstructions = null)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
