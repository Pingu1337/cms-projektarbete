using frontend.Models;
using System.Net.NetworkInformation;
using System;
using System.Text.Json;
using static frontend.Pages.Index;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace frontend.Services
{
    public interface IAuthService
    {
        Task<bool> IsSignedIn();
        Task<string> GetUsernameAsync();
        Task<User> GetUserAsync();
        Task<string> GetJwt();
        Task<int> GetRole(string Token);
        Task<bool> IsElevated(int role);
    }
    public class AuthService : IAuthService
    {
        private ILocalStorageService _localStorageService;
        private HttpClient _httpClient;


        private IConfiguration _configuration;  
        public AuthService(ILocalStorageService localStorageService, HttpClient httpClient, IConfiguration configuration)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> IsSignedIn()
        {
            var Token = await _localStorageService.GetCookie<string>("JWT");
            if (Token != null)
            {
                if (Token.StartsWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GetUsernameAsync()
        {
            var username = await _localStorageService.GetCookie<string>("Username");
            if (username == null)
            {
                throw new Exception("Username is null");
            }
            else
            {
                return username;
            }
        }
        public async Task<string> GetJwt()
        {   
            var Token = await _localStorageService.GetCookie<string>("JWT");
            if (Token == null)
            {
                return string.Empty;
            }
            else
            {
                return Token;
            }
        }
        public async Task<User> GetUserAsync()
        {
            var user = await _localStorageService.GetCookie<string>("UserData");
            if (user == null)
            {
                return default;
            }
            else
            {
                User userData = JsonSerializer.Deserialize<User>(user) ?? throw new Exception("invalid user data");
                return userData;
            }
        }
        public async Task<int> GetRole(string Token)
        {
            string strapi_api_url;

            strapi_api_url = _configuration["AppSettings:STRAPI_API_URL"];
            var url = "{STRAPI_API_URL}/api/users/me?populate=role";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetAsync(url.Replace("{STRAPI_API_URL}", strapi_api_url));
            var userData = await response.Content.ReadAsStringAsync();
            var roleData = JsonSerializer.Deserialize<Me>(userData) ?? throw new Exception("Login Response is null");

            return roleData.role.id;
        }

        public async Task<bool> IsElevated(int role)
        {
            if(role == 3)
            {
                return true;
            }
            return false;
        }
    }   


    public class Me
    {
        public int id { get; set; }
        public Role role { get; set; } 
    }

    public class Role
    {
        public int id { get; set; } 
        public string name { get; set; }
        public string type { get; set; }
    }   
}
