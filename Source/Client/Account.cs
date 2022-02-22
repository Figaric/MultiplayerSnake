using MultiplayerSnake.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    class Account
    {
        public string JwtToken { get; private set; }
        public string Nickname { get; private set; }
        public string Path { get; }
        public bool Logon { get; private set; }

        public Account()
        {
            Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\lolsquad\\MultiplayerSnake\\";
            Directory.CreateDirectory(Path);
            Logon = false;
            _ = LoadToken();
        }

        public async Task Register(string login, string password)
        {
            RestClient client = new RestClient("http://localhost:5000/account/register/");
            RestRequest request = new RestRequest().AddJsonBody(new { Username = login, Password = password });
            RestResponse response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Response: " + response.Content);


                var body = JsonConvert.DeserializeObject<ResponseFail<FieldError>>(response.Content);

                Console.WriteLine("Body: " + body.Errors.First().Message);
            }
        }

        public async Task Login(string login, string password)
        {
            RestClient client = new RestClient("http://localhost:5000/account/login/");
            RestRequest request = new RestRequest().AddJsonBody(new { Username = login, Password = password });
            RestResponse response = await client.PostAsync(request);
            JwtToken = JsonConvert.DeserializeObject<ResponseData<LoginResponseData>>(response.Content).Data.JwtToken;
            Logon = true;

            await SaveToken(JwtToken);
        }

        public async Task SaveToken(string token)
        {
            FileStream f = File.Create(Path + "Jwt");
            await f.WriteAsync(Encoding.UTF8.GetBytes(token));
            f.Close();
        }
        public async Task LoadToken()
        {
            FileStream f = File.OpenRead(Path + "Jwt");
            if (f.Length > 16)
            {
                byte[] data = new byte[f.Length];
                await f.ReadAsync(data);
                JwtToken = Encoding.UTF8.GetString(data);
                Logon = true;
            }
            f.Close();
        }
    }
}
