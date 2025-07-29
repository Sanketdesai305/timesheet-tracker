using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email, Guid? excludeUserId = null);
    }
    
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByUserAsync(Guid userId);
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
        Task<IEnumerable<Project>> GetActiveProjectsByUserAsync(Guid userId);
        Task<Project> CreateAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
    
    public interface ITimeEntryRepository
    {
        Task<TimeEntry?> GetByIdAsync(Guid id);
        Task<IEnumerable<TimeEntry>> GetAllAsync();
        Task<IEnumerable<TimeEntry>> GetByUserAsync(Guid userId);
        Task<IEnumerable<TimeEntry>> GetByProjectAsync(Guid projectId);
        Task<IEnumerable<TimeEntry>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<TimeEntry>> GetByProjectAndDateRangeAsync(Guid projectId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<TimeEntry>> GetPendingApprovalsAsync();
        Task<IEnumerable<TimeEntry>> GetActiveTimersByUserAsync(Guid userId);
        Task<TimeEntry> CreateAsync(TimeEntry timeEntry);
        Task<TimeEntry> UpdateAsync(TimeEntry timeEntry);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> HasActiveTimerAsync(Guid userId);
    }
}
