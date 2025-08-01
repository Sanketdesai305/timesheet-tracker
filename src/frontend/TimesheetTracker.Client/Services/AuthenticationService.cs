using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TimesheetTracker.Client.Models;

namespace TimesheetTracker.Client.Services
{
    public interface IAuthenticationService
    {
        event Action<bool> AuthenticationStateChanged;
        UserDto? CurrentUser { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsManager { get; }
        bool IsTeamLead { get; }
        bool IsFinanceHR { get; }
        Task<bool> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<bool> RegisterAsync(RegisterRequest request);
        Task InitializeAsync();
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiService _apiService;
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public event Action<bool>? AuthenticationStateChanged;

        public UserDto? CurrentUser { get; private set; }
        public bool IsAuthenticated => CurrentUser != null;
        public bool IsAdmin => CurrentUser?.Role == "Admin";
        public bool IsManager => CurrentUser?.Role == "Manager";
        public bool IsTeamLead => CurrentUser?.Role == "TeamLead";
        public bool IsFinanceHR => CurrentUser?.Role == "FinanceHR";

        public AuthenticationService(IApiService apiService, IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _apiService = apiService;
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            try
            {
                var result = await _apiService.LoginAsync(request);
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await SetTokenAsync(result.Token);
                    CurrentUser = result.User;
                    
                    AuthenticationStateChanged?.Invoke(true);
                    return true;
                }
            }
            catch (Exception)
            {
                // Log error
            }
            
            return false;
        }

        public async Task LogoutAsync()
        {
            await RemoveTokenAsync();
            CurrentUser = null;
            
            AuthenticationStateChanged?.Invoke(false);
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var result = await _apiService.RegisterAsync(request);
                if (result != null)
                {
                    // After successful registration, you might want to automatically log in
                    var loginRequest = new LoginRequest
                    {
                        Email = request.Email,
                        Password = request.Password
                    };
                    
                    return await LoginAsync(loginRequest);
                }
            }
            catch (Exception)
            {
                // Log error
            }
            
            return false;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var token = await GetTokenAsync();
                await _jsRuntime.InvokeVoidAsync("console.log", $"[Auth] Token from localStorage: {token}");
                if (!string.IsNullOrEmpty(token))
                {
                    SetAuthorizationHeader(token);
                    await _jsRuntime.InvokeVoidAsync("console.log", "[Auth] Authorization header set.");
                    var user = DecodeTokenToUser(token);
                    if (user != null)
                    {
                        await _jsRuntime.InvokeVoidAsync("console.log", $"[Auth] User decoded from token: {user.Email}");
                        CurrentUser = user;
                        AuthenticationStateChanged?.Invoke(true);
                    }
                    else
                    {
                        await _jsRuntime.InvokeVoidAsync("console.warn", "[Auth] Token could not be decoded to user. Logging out.");
                        await LogoutAsync();
                    }
                }
                else
                {
                    await _jsRuntime.InvokeVoidAsync("console.warn", "[Auth] No token found in localStorage.");
                }
            }
            catch (Exception ex)
            {
                await _jsRuntime.InvokeVoidAsync("console.error", $"[Auth] Exception in InitializeAsync: {ex.Message}");
                await LogoutAsync();
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                return await _apiService.ChangePasswordAsync(request);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            SetAuthorizationHeader(token);
        }

        private async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        private async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private UserDto? DecodeTokenToUser(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3) return null;

                var payload = parts[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                _ = _jsRuntime.InvokeVoidAsync("console.log", $"[Auth] Decoded JWT payload: {json}");
                if (claims == null) return null;

                string GetClaim(string key)
                    => claims.TryGetValue(key, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";

                return new UserDto
                {
                    Id = Guid.TryParse(GetClaim("userId"), out var id) ? id : Guid.Empty,
                    Email = GetClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"),
                    FirstName = GetClaim("firstName"),
                    LastName = GetClaim("lastName"),
                    Role = GetClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
                    Department = GetClaim("department")
                };
            }
            catch (Exception ex)
            {
                _ = _jsRuntime.InvokeVoidAsync("console.error", $"[Auth] Exception decoding token: {ex.Message}");
                return null;
            }
        }
    }
}
