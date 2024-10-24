using MongoDB.Bson.Serialization.Attributes;
namespace LagerhotellAPI.Models.DbModels;

public record EmailVerificationCodeDocument(int Code, string Email, DateTime TimeCreated)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}
