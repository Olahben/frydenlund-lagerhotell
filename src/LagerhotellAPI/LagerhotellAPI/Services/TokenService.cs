using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LagerhotellAPI.Services
{
    public class TokenService
    {
        private readonly JwtSecurityTokenHandler _jwtHandler = new();
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Jwt CreateJwt(string id, string phoneNumber, bool isAdministrator)
        {
            var secretKeyString = _configuration["Jwt:Key"];
            Console.WriteLine(secretKeyString);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenOptions;

            if (isAdministrator)
            {
                tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: new List<Claim>
                    {
                    new Claim(ClaimTypes.Sid, id),
                    new Claim(ClaimTypes.MobilePhone, phoneNumber),
                    new Claim(ClaimTypes.Role, "Administrator")
                    },
                    expires: DateTime.Now.AddMinutes(10000),
                    signingCredentials: signinCredentials
                );
            }
            else
            {
                tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: new List<Claim>
                    {
                    new Claim(ClaimTypes.Sid, id),
                    new Claim(ClaimTypes.MobilePhone, phoneNumber)
                    },
                    expires: DateTime.Now.AddMinutes(240),
                    signingCredentials: signinCredentials
                );
            }

            var tokenString = _jwtHandler.WriteToken(tokenOptions);
            return new Jwt { Token = tokenString };
        }

        /* public string DecodeToken(string token)
        {
            JwtSecurityToken? jsonToken = (JwtSecurityToken)_jwtHandler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                Claim? claim = jsonToken.Claims.FirstOrDefault(c => c.Type == "MobilePhone");
                return claim?.Value;
            }

            throw new Exception("Ikke noe mobilnummer i tokenen");
        } */
    }
}
