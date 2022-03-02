using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;

namespace MultiplayerSnake.Server
{
    public static class Utillities
    {
        public static async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var body = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return body;
        }

        public static IApplicationBuilder UseConditionalMiddleware<TMiddleware>(this WebApplication app, string route)
        {
            return app.UseWhen(context => context.Request.Path.StartsWithSegments('/' + route), appBuilder =>
            {
                appBuilder.UseMiddleware<TMiddleware>();
            });
        }

        public static void SendEmail(string htmlBody)
        {
            var message = new MailMessage();
            var smtpClient = new SmtpClient();

            message.From = new MailAddress("");
            message.To.Add(new MailAddress(""));

            message.Subject = "Change password";
            message.IsBodyHtml = true;
            message.Body = htmlBody;

            smtpClient.Port = 587;  
            smtpClient.Host = "smtp.gmail.com"; //for gmail host  
            smtpClient.EnableSsl = true;  
            smtpClient.UseDefaultCredentials = false;  
            smtpClient.Credentials = new NetworkCredential("FromMailAddress", "password");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;  
            smtpClient.Send(message); 
        }

        public static string GenerateJwtToken(User user, JwtSettings jwtSettings)
        {
            var claims = new[]
    {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

                // Add user Id so that UserManager.GetUserAsync can find the user based on Id
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMonths(3)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
