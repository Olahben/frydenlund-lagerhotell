using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels.Auth0;

public record RefreshTokenDocument(string RefreshToken, string UserAuth0Id)
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; init; }
}