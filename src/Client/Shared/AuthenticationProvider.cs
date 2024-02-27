using Blazored.LocalStorage;
using Domain.Users;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Client.Shared
{
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _http;

        public AuthenticationProvider(ILocalStorageService localStorageService, IConfiguration configuration, HttpClient http)
        {
            _localStorageService = localStorageService;
            _configuration = configuration;
            _http = http;
        }
        public static ClaimsPrincipal Customer(string firstname, string email, int id) => new(new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, firstname),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "Customer"),
            new Claim(ClaimTypes.NameIdentifier, id.ToString() )
        }, "Customer"));

        public static ClaimsPrincipal Admin(string firstname, string email, AdminRole role,int id) => new(new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, firstname),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, id.ToString() )

        }, "Admin"));

        public static ClaimsPrincipal Anonymous => new(new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, "Guest"),
            new Claim(ClaimTypes.Role, "Guest"),

        }, "Guest"));

        public ClaimsPrincipal Current { get; set; } = Anonymous;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _localStorageService.GetItemAsStringAsync("jwt_token");

            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            }
            
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            ChangeAuthenticationState(user);
           

            return state;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(MapJsonKeyToClaimType(kvp.Key), kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private static string MapJsonKeyToClaimType(string jsonKey)
        {
            switch (jsonKey)
            {
                case "email":
                    return ClaimTypes.Email;
                case "nameid":
                    return ClaimTypes.NameIdentifier;
                case "role":
                    return ClaimTypes.Role;
                case "name":
                    return ClaimTypes.Name;
                case "exp":
                    return ClaimTypes.Expiration;
                default:
                    return jsonKey;
            }
        }
        public void ChangeAuthenticationState(ClaimsPrincipal claims)
        {
            var state = new AuthenticationState(claims);

            Current = claims;
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }
    }
}