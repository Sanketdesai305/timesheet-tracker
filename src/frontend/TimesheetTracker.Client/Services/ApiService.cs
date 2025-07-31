using System.Net.Http.Json;
using System.Text.Json;
using TimesheetTracker.Client.Models;

namespace TimesheetTracker.Client.Services
{
    public partial interface IApiService
    {
        // Authentication
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<UserDto?> RegisterAsync(RegisterRequest request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);

        // Users
        Task<List<UserDto>?> GetUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> CreateUserAsync(CreateUserRequest request);
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(Guid id);

        // Projects
        Task<List<ProjectDto>?> GetProjectsAsync();
        Task<List<ProjectDto>?> GetActiveProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(Guid id);
        Task<ProjectDto?> CreateProjectAsync(CreateProjectRequest request);
        Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectRequest request);
        Task<bool> DeleteProjectAsync(Guid id);

        // Time Entries
        Task<List<TimeEntryDto>?> GetTimeEntriesAsync();
        Task<List<TimeEntryDto>?> GetMyTimeEntriesAsync();
        Task<List<TimeEntryDto>?> GetTimeEntriesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<TimeEntryDto?> GetTimeEntryByIdAsync(Guid id);
        Task<TimeEntryDto?> CreateTimeEntryAsync(CreateTimeEntryRequest request);
        Task<TimeEntryDto?> UpdateTimeEntryAsync(Guid id, UpdateTimeEntryRequest request);
        Task<bool> DeleteTimeEntryAsync(Guid id);
        Task<TimeEntryDto?> StartTimerAsync(StartTimerRequest request);
        Task<bool> StopTimerAsync(Guid timeEntryId);
        Task<List<TimeEntryDto>?> GetActiveTimersAsync();
        Task<List<TimeEntryDto>?> GetPendingApprovalsAsync();
        Task<TimeEntryDto?> ApproveTimeEntryAsync(Guid id, ApproveTimeEntryRequest request);

        // Reports
        Task<List<ShiftInstanceDto>?> GetShiftInstancesAsync();
        Task<List<TimeEntryReportDto>?> GetTimeEntryReportAsync(DateTime startDate, DateTime endDate);
        Task<List<UserTimeReportDto>?> GetUserTimeReportAsync(DateTime startDate, DateTime endDate);
        Task<List<ProjectTimeReportDto>?> GetProjectTimeReportAsync(DateTime startDate, DateTime endDate);
    }

    public partial class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // Authentication
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>(_jsonOptions);
                return apiResponse?.Data;
            }
            return null;
        }

        public async Task<UserDto?> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>(_jsonOptions);
                    return apiResponse?.Data?.User;
                }
                
                // Log the error response for debugging
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Registration failed: {response.StatusCode} - {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", request);
            return response.IsSuccessStatusCode;
        }

        // Users
        public async Task<List<UserDto>?> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users", _jsonOptions);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{id}", _jsonOptions);
        }

        public async Task<UserDto?> CreateUserAsync(CreateUserRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions) : null;
        }

        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions) : null;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            return response.IsSuccessStatusCode;
        }

        // Projects
        public async Task<List<ProjectDto>?> GetProjectsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProjectDto>>>("api/projects", _jsonOptions);
            return response?.Data;
        }

        public async Task<List<ProjectDto>?> GetActiveProjectsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProjectDto>>>("api/projects/active", _jsonOptions);
            return response?.Data;
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ProjectDto>($"api/projects/{id}", _jsonOptions);
        }

        public async Task<ProjectDto?> CreateProjectAsync(CreateProjectRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProjectDto>(_jsonOptions) : null;
        }

        public async Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{id}", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProjectDto>(_jsonOptions) : null;
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{id}");
            return response.IsSuccessStatusCode;
        }

        // Time Entries
        public async Task<List<TimeEntryDto>?> GetTimeEntriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TimeEntryDto>>("api/timeentries", _jsonOptions);
        }

        public async Task<List<TimeEntryDto>?> GetMyTimeEntriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<TimeEntryDto>>>("api/timeentries", _jsonOptions);
            return response?.Data;
        }

        public async Task<List<TimeEntryDto>?> GetTimeEntriesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<TimeEntryDto>>>(
                $"api/timeentries/range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", _jsonOptions);
            return response?.Data;
        }

        public async Task<TimeEntryDto?> GetTimeEntryByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<TimeEntryDto>($"api/timeentries/{id}", _jsonOptions);
        }

        public async Task<TimeEntryDto?> CreateTimeEntryAsync(CreateTimeEntryRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/timeentries", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<TimeEntryDto>(_jsonOptions) : null;
        }

        public async Task<TimeEntryDto?> UpdateTimeEntryAsync(Guid id, UpdateTimeEntryRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/timeentries/{id}", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<TimeEntryDto>(_jsonOptions) : null;
        }

        public async Task<bool> DeleteTimeEntryAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/timeentries/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<TimeEntryDto?> StartTimerAsync(StartTimerRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/timeentries/start", request);
            if (!response.IsSuccessStatusCode) return null;
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TimeEntryDto>>(_jsonOptions);
            return apiResponse?.Data;
        }

        public async Task<bool> StopTimerAsync(Guid timeEntryId)
        {
            var response = await _httpClient.PostAsync($"api/timeentries/{timeEntryId}/stop", null);
            if (!response.IsSuccessStatusCode) return false;
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TimeEntryDto>>(_jsonOptions);
            return apiResponse?.Success == true;
        }

        public async Task<List<TimeEntryDto>?> GetActiveTimersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<TimeEntryDto>>>("api/timeentries/active", _jsonOptions);
            return response?.Data;
        }

        public async Task<List<TimeEntryDto>?> GetPendingApprovalsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TimeEntryDto>>("api/timeentries/pending-approvals", _jsonOptions);
        }

        public async Task<TimeEntryDto?> ApproveTimeEntryAsync(Guid id, ApproveTimeEntryRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/timeentries/{id}/approve", request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<TimeEntryDto>(_jsonOptions) : null;
        }

        // Reports
        public async Task<List<TimeEntryReportDto>?> GetTimeEntryReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _httpClient.GetFromJsonAsync<List<TimeEntryReportDto>>(
                $"api/reports/time-entries?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", _jsonOptions);
        }

        public async Task<List<UserTimeReportDto>?> GetUserTimeReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _httpClient.GetFromJsonAsync<List<UserTimeReportDto>>(
                $"api/reports/user-time?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", _jsonOptions);
        }

        public async Task<List<ProjectTimeReportDto>?> GetProjectTimeReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectTimeReportDto>>(
                $"api/reports/project-time?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", _jsonOptions);
        }
    }
}
