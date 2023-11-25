using Domain.Modals;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Web_API.Middleware.Auth
{
    public class JWTAuthManager : IJWTAuthManager
    {
        private readonly IConfiguration _configuration;

        public JWTAuthManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWT(User user)
        {
            //SymmetricSecurityKey :- return new instance 
            //Encoding :- Represents the charecter encoading
            //UTF8 :- gets the encoding for the UTF-8 format
            //GetBytes :- a byte array containing the result of encoding the specified set of charecter
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            //SigningCredentials :- Initialize new instances of SigningCredentials
            //SecurityAlgorithms :- Constants for the security algoeithm
            //(HmacSha256) :- constant string HS256 
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Initialize new instances of JwtSecurityToken
            var token = new JwtSecurityToken(
                _configuration["Jwt : Issuer"],
                _configuration["Jwt : Issuer"],
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
