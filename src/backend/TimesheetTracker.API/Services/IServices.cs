using TimesheetTracker.API.DTOs;
using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
    
    public interface IProjectService
    {
        Task<ProjectDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<IEnumerable<ProjectDto>> GetByUserAsync(Guid userId);
        Task<IEnumerable<ProjectDto>> GetActiveProjectsAsync();
        Task<IEnumerable<ProjectDto>> GetActiveProjectsByUserAsync(Guid userId);
        Task<ProjectDto> CreateAsync(CreateProjectDto createProjectDto, Guid createdBy);
        Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto updateProjectDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
    
    public interface ITimeEntryService
    {
        Task<TimeEntryDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<TimeEntryDto>> GetAllAsync();
        Task<IEnumerable<TimeEntryDto>> GetByUserAsync(Guid userId);
        Task<IEnumerable<TimeEntryDto>> GetByProjectAsync(Guid projectId);
        Task<IEnumerable<TimeEntryDto>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<TimeEntryDto>> GetPendingApprovalsAsync();
        Task<IEnumerable<TimeEntryDto>> GetActiveTimersByUserAsync(Guid userId);
        Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto createTimeEntryDto, Guid userId);
        Task<TimeEntryDto> StartTimerAsync(StartTimeEntryDto startTimeEntryDto, Guid userId);
        Task<TimeEntryDto> StopTimerAsync(Guid timeEntryId, DateTime? endTime = null);
        Task<TimeEntryDto> UpdateAsync(Guid id, UpdateTimeEntryDto updateTimeEntryDto, Guid userId);
        Task<TimeEntryDto> ApproveAsync(Guid id, Guid approverId);
        Task DeleteAsync(Guid id, Guid userId);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> HasActiveTimerAsync(Guid userId);
    }
    
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<bool> ValidatePasswordAsync(string password, string hashedPassword);
        string HashPassword(string password);
    }
    
    public interface IReportService
    {
        Task<TimeReportDto> GenerateUserTimeReportAsync(Guid userId, DateTime startDate, DateTime endDate, bool onlyApproved = false);
        Task<ProjectReportDto> GenerateProjectReportAsync(Guid projectId, DateTime startDate, DateTime endDate);
        Task<TimeReportDto> GenerateTeamTimeReportAsync(DateTime startDate, DateTime endDate, bool onlyApproved = false);
        Task<byte[]> ExportToExcelAsync(TimeReportDto report);
        Task<byte[]> ExportToPdfAsync(TimeReportDto report);
    }
}
