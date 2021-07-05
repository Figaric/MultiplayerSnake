using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiplayerSnake.Server
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Validated <paramref name="accessToken"/>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>If the tokens signature is valid, returns userId, otherwise returns 0</returns>
        public int GetAccessTokenPayload(string accessToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                // Validate issuer
                ValidateIssuer = true,
                // Validate audience
                ValidateAudience = true,
                // Validate expiration
                ValidateLifetime = true,
                // Validate signature
                ValidateIssuerSigningKey = true,
                // Set issuer
                ValidIssuer = _configuration["Jwt:Issuer"],
                // Set audience
                ValidAudience = _configuration["Jwt:Audience"],

                // Set signing key
                IssuerSigningKey = new SymmetricSecurityKey(
                    // Get our secret key from configuration
                    Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
            };

            ClaimsPrincipal payload;

            try
            {
                payload = new JwtSecurityTokenHandler().ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);
            } 
            catch(SecurityTokenException)
            {
                return 0;
            }

            int userId = int.Parse(payload.FindFirstValue(ClaimTypes.NameIdentifier));

            return userId;
        }

        public string CreateAccessToken(int userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(
                // Get the secret key from configuration
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:AccessTokenSecret"])),
                // Use HS256 algorithm
                SecurityAlgorithms.HmacSha256);

            // Generate the Jwt Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials,
                // Expire if not used for 3 months
                expires: DateTime.Now.AddDays(14));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
