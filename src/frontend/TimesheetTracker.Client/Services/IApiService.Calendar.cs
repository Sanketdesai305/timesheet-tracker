using TimesheetTracker.Client.Models;

namespace TimesheetTracker.Client.Services
{
    public partial interface IApiService
    {
        Task<List<ShiftTemplateDto>?> GetShiftTemplatesAsync();
        Task<List<HolidayDto>?> GetHolidaysAsync();
    }
}
