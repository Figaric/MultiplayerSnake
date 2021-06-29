using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateAccessToken(int userId)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AccessTokenSecret"]));
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = System.DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    symmetricKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
