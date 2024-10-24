global using LagerhotellAPI.Models.DbModels.Auth0;
using Amazon.Runtime.Internal;
using LagerhotellAPI.Models;
using LagerhotellAPI.Repositories;

namespace LagerhotellAPI.Services
{
    public class UserRepository
    {
        private readonly IMongoCollection<Models.DbModels.User> _users;
        private readonly CompanyUserService _companyUserService;
        private readonly Auth0UserService _auth0UserService;

        public UserRepository(MongoDbSettings settings, IConfiguration configuration, RefreshTokens repos, TokenService tokenService, Auth0UserService auth0UserService)
        {
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ArgumentNullException(nameof(settings), "MongoDbSettings is not configured properly.");


            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _users = database.GetCollection<Models.DbModels.User>("Users");
            _auth0UserService = new Auth0UserService(configuration);
            _companyUserService = new CompanyUserService(settings, tokenService, auth0UserService, configuration, repos);
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthDate"></param>
        /// <param name="streetAddress"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="password"></param>
        /// <param name="isAdministrator"></param>
        /// <returns>user object</returns>
        public async Task<(User, string, string)> Add(string firstName, string lastName, string phoneNumber, string birthDate, string streetAddress, string postalCode, string city, string password, bool isAdministrator, string email)
        {
            Address userAddress = new(streetAddress, postalCode, city);
            string userId = Guid.NewGuid().ToString();
            await AddUserToAuth0(new User(userId, firstName, lastName, phoneNumber, birthDate, userAddress, password, isAdministrator, email, false, ""));
            (string accessToken, string refreshToken) = await _auth0UserService.GetUserTokens(password, email);
            string auth0Id = await _auth0UserService.GetUserIdViaToken(accessToken);

            Models.DbModels.User user = new(userId, firstName, lastName, phoneNumber, birthDate, userAddress, isAdministrator, email, false, auth0Id);
            await _users.InsertOneAsync(user);
            // empty password to avoid sending it back to the client
            User domainUser = new(user.UserId, user.FirstName, user.LastName, phoneNumber, user.BirthDate, userAddress, "", user.IsAdministrator, email, false, auth0Id);
            return (domainUser, accessToken, refreshToken);

        }

        public async Task AddUserToAuth0(User user)
        {
            UserAuth0 userAuth0 = new(user.Id, user.Email, user.IsEmailVerified)
            {
                Password = user.Password
            };
            await _auth0UserService.AddUser(userAuth0, false, user.IsAdministrator);
        }

        /// <summary>
        /// Gets a user from the database with the given phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>user object</returns>
        public User? Get(string phoneNumber)
        {
            var dbUser = _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            // empty password to avoid sending it back to the client
            return new LagerhotellAPI.Models.DomainModels.User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, "", dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified, dbUser.Auth0Id);
        }

        public User? GetByEmail(string email)
        {
            var dbUser = _users.Find(User => User.Email == email).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            // empty password to avoid sending it back to the client
            return new LagerhotellAPI.Models.DomainModels.User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, "", dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified, dbUser.Auth0Id);
        }

        public async Task<bool> DoesSimilarUserExist(string phoneNumber, string email)
        {
            User user = GetByEmail(email);
            if (user != null)
            {
                return true;
            }
            User user1 = Get(phoneNumber);
            if (user1 != null)
            {
                return true;
            }
            try
            {
                await _companyUserService.GetCompanyUserByEmail(email);
                return true;
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    await _companyUserService.GetCompanyUserByPhoneNumber(phoneNumber);
                    return true;
                }
                catch (KeyNotFoundException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a user from the database with the given phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>Database user object</returns>
        public Models.DbModels.User? GetByPhoneDbModel(string phoneNumber)
        {
            var dbUser = _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return dbUser;
        }

        /// <summary>
        /// Gets a users password from the database with the given phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>user password</returns>
        public string? Password(string phoneNumber)
        {
            var user = Get(phoneNumber);
            if (user == null)
            {
                return null;
            }
            return user.Password;
        }

        /// <summary>
        /// Gets a user in the database with the given user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user object</returns>
        public User? GetUserById(string id)
        {
            var dbUser = _users.Find(user => user.UserId == id).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            // empty password to avoid sending it back to the client
            return new User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, "", dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified, dbUser.Auth0Id);
        }

        public LagerhotellAPI.Models.DbModels.User? GetUserByIdDbModel(string id)
        {
            var dbUser = _users.Find(user => user.UserId == id).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return dbUser;
        }

        /// <summary>
        /// Checks if the given password matches the password in the database
        /// </summary>
        /// <param name="password"></param>
        /// <param name="requestedPassword"></param>
        /// <returns>bool</returns>
        public bool DoPasswordsMatch(string password, string requestedPassword)
        {
            return password == requestedPassword;
        }

        /// <summary>
        /// Updates a users data (except for phone number and Id) in the database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthDate"></param>
        /// <param name="password"></param>
        /// <param name="streetAddress"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="isAdministrator"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void UpdateUserValues(string userId, string firstName, string lastName, string phoneNumber, string birthDate, string password, string streetAddress, string postalCode, string city, bool isAdministrator, string email, bool isEmailVerified)
        {
            Models.DbModels.User oldDbUser = GetUserByIdDbModel(userId);
            if (oldDbUser != null)
            {
                Address userAddress = new(streetAddress, postalCode, city);
                Models.DbModels.User updatedUserDb = new(oldDbUser.Id, oldDbUser.UserId, firstName, lastName, oldDbUser.PhoneNumber, birthDate, userAddress, isAdministrator, email, isEmailVerified, oldDbUser.Auth0Id);
                var filter = Builders<Models.DbModels.User>.Filter.Eq(user => user.UserId, oldDbUser.UserId);
                var options = new ReplaceOptions { IsUpsert = false };
                _users.ReplaceOne(filter, updatedUserDb, options);
            }
            else
            {
                throw new KeyNotFoundException("Brukeren som skulle oppdateres ble ikke funnet");
            }
        }

        public async Task<List<User>> GetAllUsers(int? skip, int? take)
        {
            var dbUsers = await _users.Find(_ => true).Limit(take).Skip(skip).ToListAsync();
            // empty password to avoid sending it back to the client
            List<User> domainUsers = dbUsers.ConvertAll(dbUser =>
            {
                return new User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, "", dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified, dbUser.Auth0Id);
            });
            return domainUsers;
        }

        public async Task DeleteUser(string id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                await _users.DeleteOneAsync(user => user.UserId == id);
            }
            else
            {
                throw new KeyNotFoundException("User not found.");
            }
        }

        public async Task<User> GetByAuth0Id(string id)
        {
            var dbUser = await _users.Find(user => user.Auth0Id == id).FirstOrDefaultAsync();
            if (dbUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            // empty password to avoid sending it back to the client
            return new User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, "", dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified, dbUser.Auth0Id);
        }
    }
}
