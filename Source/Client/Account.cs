using MultiplayerSnake.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace MultiplayerSnake.Client
{
    class Account
    {
        public string JwtToken { get; private set; }
        public string Nickname { get; private set; }
        public string Path { get; }
        public bool Logon { get; private set; }
        public ErrorH Error { get; set; }

        public Account()
        {
            Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\lolsquad\\MultiplayerSnake\\";
            Directory.CreateDirectory(Path);
            Logon = false;
            JwtToken = null;
            _ = LoadToken();
            Error = new ErrorH();
            Error.CurrentError = Errors.Success;
        }

        public async Task Register(string login, string password)
        {
            RestClient client = new RestClient("http://localhost:5000/account/register/");
            RestRequest request = new RestRequest().AddJsonBody(new { Username = login, Password = password });
            RestResponse response = await client.ExecutePostAsync(request);

            if (response.IsSuccessful)
            {
                Error.CurrentError = Errors.Success;
                await Login(login, password);
            }
            else
            {
                var body = JsonConvert.DeserializeObject<ResponseFail<FieldError>>(response.Content);
                if (body.Errors.ToArray()[0].Message == "This UserName is already taken")
                {
                    Error.CurrentError = Errors.UserAlreadyExists;
                    Error.Print(false);
                }
            }
        }

        public async Task Login(string login, string password)
        {
            RestClient client = new RestClient("http://localhost:5000/account/login/");
            RestRequest request = new RestRequest().AddJsonBody(new { Username = login, Password = password });
            RestResponse response = await client.ExecutePostAsync(request);

            if (response.IsSuccessful)
            {
                Error.CurrentError = Errors.Success;
                Error.Print(true);
                JwtToken = JsonConvert.DeserializeObject<ResponseData<LoginResponseData>>(response.Content).Data.JwtToken;
                await SaveToken(JwtToken);
            }
            else
            {
                var body = JsonConvert.DeserializeObject<ResponseFail<FieldError>>(response.Content);
                if (body.Errors[0].Message == "Such user does not exist")
                {
                    Error.CurrentError = Errors.SuchUserDoesNotExist;
                    Error.Print(true);
                }
                else if (body.Errors[0].Message == "Invalid password")
                {
                    Error.CurrentError = Errors.InvalidPassword;
                    Error.Print(true);
                }
            }
        }

        public async Task SaveToken(string token)
        {
            FileStream f = File.Create(Path + "Jwt");
            f.Write(Encoding.UTF8.GetBytes(token));
            f.Close();
            Logon = true;
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
            else
            {
                JwtToken = null;
                Logon = false;
            }
            f.Close();
        }

        public async Task Logout()
        {
            File.Delete(Path + "Jwt");
            JwtToken = null;
            Logon = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t\tВы успешно вышли из аккаунта");
            Console.ResetColor();
            Console.WriteLine("\n\t\tДалее - любая клавиша");
        }
    }
}
