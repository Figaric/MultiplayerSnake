using RestSharp;

var client = new RestClient("http://localhost:5000/account/register/");
var request = new RestRequest().AddJsonBody(new { Username = "wadwad", Password = "wdwiqdqwd" });
var response = await client.PostAsync(request);
Console.WriteLine(response);