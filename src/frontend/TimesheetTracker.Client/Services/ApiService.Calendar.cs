using System.Net.Http.Json;
using TimesheetTracker.Client.Models;

namespace TimesheetTracker.Client.Services
{
    public partial class ApiService
    {
        public async Task<List<ShiftTemplateDto>?> GetShiftTemplatesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ShiftTemplateDto>>("api/ShiftTemplates");
        }

        public async Task<List<HolidayDto>?> GetHolidaysAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<HolidayDto>>("api/Holidays");
        }
        public async Task<List<ShiftInstanceDto>?> GetShiftInstancesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ShiftInstanceDto>>("api/ShiftInstances");
        }
    }
}
