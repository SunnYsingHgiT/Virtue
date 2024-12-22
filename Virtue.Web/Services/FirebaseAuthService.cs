using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Virtue.Web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Virtue.Web.Services
{
    public class FirebaseAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FirebaseAuthService(FirebaseSettings firebaseSettings)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://identitytoolkit.googleapis.com/")
            };
            _apiKey = firebaseSettings.ApiKey;
        }

        public async Task<string> LoginWithEmailAndPasswordAsync(string email, string password)
        {
            var requestBody = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsync(
                $"v1/accounts:signInWithPassword?key={_apiKey}",
                new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Firebase login failed: {response.ReasonPhrase}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(responseBody);

            return responseJson.GetProperty("idToken").GetString();
        }

        public async Task<string> RegisterUserAsync(string email, string password)
        {
            var requestBody = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var myContent = JsonConvert.SerializeObject(requestBody);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PostAsync(
                $"v1/accounts:signUp?key={_apiKey}", byteContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Firebase registration failed: {response.ReasonPhrase}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            //var responseJson = JsonSerializer.Deserialize<JsonElement>(responseBody);

            return responseBody.ToString();
        }
    }
}
