global using MongoDB.Driver;
global using LagerhotellAPI.Models.DbModels.Auth0;
global using LagerhotellAPI.Models;
using LagerhotellAPI.Models.DbModels;
namespace LagerhotellAPI.Repositories;

public class RefreshTokens
{
    private readonly IMongoCollection<Models.DbModels.Auth0.RefreshTokenDocument> _refreshTokens;

    public RefreshTokens(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _refreshTokens = database.GetCollection<Models.DbModels.Auth0.RefreshTokenDocument>("RefreshTokens");
    }

    public async Task<Models.DbModels.Auth0.RefreshTokenDocument?> GetRefreshToken(string auth0Id)
    {
        Models.DbModels.Auth0.RefreshTokenDocument? refreshTokenDocument = await _refreshTokens.Find(refreshTokenDocument => refreshTokenDocument.UserAuth0Id == auth0Id).FirstOrDefaultAsync();
        return refreshTokenDocument;
    }

    public async Task CreateRefreshToken(Models.DbModels.Auth0.RefreshTokenDocument refreshTokenDocument)
    {
        await _refreshTokens.InsertOneAsync(refreshTokenDocument);
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        await _refreshTokens.DeleteOneAsync(refreshTokenDocument => refreshTokenDocument.RefreshToken == refreshToken);
    }
}
