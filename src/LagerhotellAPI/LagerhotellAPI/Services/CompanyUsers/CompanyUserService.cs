global using MongoDB.Driver;
using LagerhotellAPI.Models;
using LagerhotellAPI.Models.DbModels;
using System.Data.SqlTypes;

namespace LagerhotellAPI.Services
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly IMongoCollection<Models.DbModels.CompanyUserDocument> _companyUsers;
        private readonly IMongoCollection<Models.DbModels.User> _users;
        private readonly TokenService _tokenService;
        private readonly Auth0UserService _auth0UserService;
        private readonly RefreshTokens _refreshTokenRepository;
        private readonly string _bronnoysundApiUrl = "https://data.brreg.no/enhetsregisteret/api";

        public CompanyUserService(MongoDbSettings settings, TokenService tokenService, Auth0UserService auth0UserService, IConfiguration configuration, RefreshTokens refreshTokenRepository)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _companyUsers = database.GetCollection<CompanyUserDocument>("CompanyUsers");
            _users = database.GetCollection<Models.DbModels.User>("Users");
            _tokenService = new TokenService(configuration);
            _auth0UserService = new Auth0UserService(configuration);
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<CompanyUser> GetCompanyUserAsync(string id)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(cu => cu.CompanyUserId == id).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            // empty password to avoid sending it to the client
            return new CompanyUser(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, "", dbCompanyUserDocument.IsEmailVerified, dbCompanyUserDocument.Auth0Id);
        }

        public async Task<CompanyUserDocument> GetCompanyUserDocument(string id)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(cu => cu.CompanyUserId == id).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            return dbCompanyUserDocument;
        }

        public async Task<List<CompanyUser>> GetCompanyUsersAsync(int? take, int? skip)
        {
            var query = _companyUsers.Find(cu => true);
            if (take.HasValue)
            {
                query = query.Limit(take.Value);
            }
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            List<CompanyUserDocument> dbCompanyUsersDocuments = await query.ToListAsync();
            return dbCompanyUsersDocuments.Select(cu => new CompanyUser(cu.CompanyUserId, cu.FirstName, cu.LastName, cu.Name, cu.CompanyNumber, cu.Email, cu.PhoneNumber, cu.Address, "", cu.IsEmailVerified, cu.Auth0Id)).ToList();
        }

        public async Task<CompanyUser> GetCompanyUserByPhoneNumber(string phoneNumber)
        {
            var dbCompanyUserDocument = await _companyUsers.Find(cu => cu.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            // empty password to avoid sending it to the client
            CompanyUser companyUser = new(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, "", dbCompanyUserDocument.IsEmailVerified, dbCompanyUserDocument.Auth0Id);
            return companyUser;
        }

        public async Task<CompanyUser> GetCompanyUserByEmail(string email)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(c => c.Email == email).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            // empty password to avoid sending it to the client
            CompanyUser companyUser = new(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, "", dbCompanyUserDocument.IsEmailVerified, dbCompanyUserDocument.Auth0Id);
            return companyUser;
        }

        public async Task<CompanyUser> GetCompanyUserByCompanyNumber(string companyNumber)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(c => c.CompanyNumber == companyNumber).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            // empty password to avoid sending it to the client
            CompanyUser companyUser = new(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, "", dbCompanyUserDocument.IsEmailVerified, dbCompanyUserDocument.Auth0Id);
            return companyUser;
        }

        public async Task<bool> DoesCompanyExistInNorway(string orgNr)
        {
            var client = new HttpClient();
            string url = $"{_bronnoysundApiUrl}/enheter/{orgNr}";
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a user with similar credentials exist
        /// </summary>
        /// <param name="companyNumber"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> DoesSimilarUserExist(string companyNumber, string phoneNumber, string email)
        {
            bool companyInNorway = await DoesCompanyExistInNorway(companyNumber);
            if (!companyInNorway)
            {
                throw new KeyNotFoundException("Company not found in Norway");
            }
            try
            {
                // Cross check if normal user has any of the same credentials
                var userExistence1 = await _users.Find(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
                var userExistence2 = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
                if (userExistence1 != null || userExistence2 != null)
                {
                    throw new SqlAlreadyFilledException();
                }
                try
                {
                    await GetCompanyUserByPhoneNumber(phoneNumber);
                    throw new SqlAlreadyFilledException();
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        await GetCompanyUserByEmail(email);
                        throw new SqlAlreadyFilledException();
                    }
                    catch (KeyNotFoundException)
                    {
                        try
                        {
                            await GetCompanyUserByCompanyNumber(companyNumber);
                            throw new SqlAlreadyFilledException();
                        }
                        catch (KeyNotFoundException)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (SqlAlreadyFilledException)
            {
                return true;
            }
        }
        public async Task<(string, string)> CreateCompanyUserAsync(CompanyUser companyUser)
        {
            bool doesSimilarUserExist;
            try
            {
                doesSimilarUserExist = await DoesSimilarUserExist(companyUser.CompanyNumber, companyUser.PhoneNumber, companyUser.Email);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company not found in Norway");
            }
            if (!doesSimilarUserExist)
            {
                string id = Guid.NewGuid().ToString();
                await _auth0UserService.AddUser(new UserAuth0(id, companyUser.Email, companyUser.IsEmailVerified) { Password = companyUser.Password }, true, false);
                (string userAccessToken, string refreshToken) = await _auth0UserService.GetUserTokens(companyUser.Password, companyUser.Email);
                string auth0Id = await _auth0UserService.GetUserIdViaToken(userAccessToken);

                var companyUserDocument = new CompanyUserDocument(id, companyUser.FirstName, companyUser.LastName, companyUser.Name, companyUser.CompanyNumber, companyUser.Email, companyUser.PhoneNumber, companyUser.Address, companyUser.IsEmailVerified, auth0Id);
                await _companyUsers.InsertOneAsync(companyUserDocument);
                await _refreshTokenRepository.CreateRefreshToken(new RefreshTokenDocument(refreshToken, auth0Id));
                return (id, userAccessToken);
            }
            throw new SqlAlreadyFilledException("User already exists");
        }

        public async Task UpdateCompanyUserAsync(string id, CompanyUser companyUser)
        {
            CompanyUserDocument existingCompanyUser;
            try
            {
                existingCompanyUser = await GetCompanyUserDocument(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            var companyUserDocument = new CompanyUserDocument(existingCompanyUser.Id, existingCompanyUser.CompanyUserId, companyUser.FirstName, companyUser.LastName, companyUser.Name, companyUser.CompanyNumber, companyUser.Email, companyUser.PhoneNumber, companyUser.Address, companyUser.IsEmailVerified, existingCompanyUser.Auth0Id);
            await _companyUsers.ReplaceOneAsync(cu => cu.CompanyUserId == id, companyUserDocument);
        }

        public async Task DeleteCompanyUserAsync(string id)
        {
            try
            {
                var companyUser = await GetCompanyUserAsync(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            await _companyUsers.DeleteOneAsync(cu => cu.CompanyUserId == id);
        }

        public async Task<(string, string)> LoginCompanyUserByEmail(string email, string password)
        {
            CompanyUser relevantCompanyUser;
            try
            {
                relevantCompanyUser = await GetCompanyUserByEmail(email);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            if (relevantCompanyUser.Password != password)
            {
                throw new SqlTypeException("Incorrect password");
            }

            string token = _tokenService.CreateJwt(relevantCompanyUser.CompanyUserId, relevantCompanyUser.PhoneNumber, false).Token;
            return (token, relevantCompanyUser.CompanyUserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="SqlAlreadyFilledException"></exception>
        /// <exception cref="SqlTypeException"></exception>
        public async Task ResetPassword(string id, string newPassword, string oldPassword)
        {
            /*CompanyUserDocument existingCompanyUser;
            try
            {
                existingCompanyUser = await GetCompanyUserDocument(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            if (existingCompanyUser.Password == newPassword)
            {
                throw new SqlAlreadyFilledException("New password is the same as the old one");
            }
            if (existingCompanyUser.Password != oldPassword)
            {
                throw new SqlTypeException("Incorrect password");
            }
            CompanyUser updatedCompanyUser = new(existingCompanyUser.CompanyUserId, existingCompanyUser.FirstName, existingCompanyUser.LastName, existingCompanyUser.Name, existingCompanyUser.CompanyNumber, existingCompanyUser.Email, existingCompanyUser.PhoneNumber, existingCompanyUser.Address, newPassword, existingCompanyUser.IsEmailVerified, existingCompanyUser.Auth0Id);
            try
            {
                await UpdateCompanyUserAsync(id, updatedCompanyUser);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }*/
        }

        public async Task<CompanyUser> GetUserByAuth0Id(string id)
        {
            var user = await _companyUsers.Find(u => u.Auth0Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            // empty password to avoid sending it to the client
            return new CompanyUser(user.CompanyUserId, user.FirstName, user.LastName, user.Name, user.CompanyNumber, user.Email, user.PhoneNumber, user.Address, "", user.IsEmailVerified, user.Auth0Id);
        }
    }
}
