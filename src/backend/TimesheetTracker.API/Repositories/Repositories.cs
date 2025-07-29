using Microsoft.EntityFrameworkCore;
using TimesheetTracker.API.Data;
using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Repositories
{
    // Repository Implementations
    public class UserRepository : IUserRepository
    {
        private readonly TimesheetDbContext _context;
        
        public UserRepository(TimesheetDbContext context)
        {
            _context = context;
        }
        
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Projects)
                .Include(u => u.TimeEntries)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
        
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }
        
        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
        
        public async Task<bool> EmailExistsAsync(string email, Guid? excludeUserId = null)
        {
            var query = _context.Users.Where(u => u.Email.ToLower() == email.ToLower());
            
            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.Id != excludeUserId.Value);
            }
            
            return await query.AnyAsync();
        }
    }
    
    public class ProjectRepository : IProjectRepository
    {
        private readonly TimesheetDbContext _context;

        public ProjectRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.TimeEntries)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.TimeEntries)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByUserAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.TimeEntries)
                .Where(p => p.CreatedBy == userId || p.TimeEntries.Any(te => te.UserId == userId))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.TimeEntries)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsByUserAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.TimeEntries)
                .Where(p => p.IsActive && (p.CreatedBy == userId || p.TimeEntries.Any(te => te.UserId == userId)))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Project> CreateAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }
    }
    
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly TimesheetDbContext _context;
        
        public TimeEntryRepository(TimesheetDbContext context)
        {
            _context = context;
        }
        
        public async Task<TimeEntry?> GetByIdAsync(Guid id)
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Include(te => te.Approver)
                .FirstOrDefaultAsync(te => te.Id == id);
        }
        
        public async Task<IEnumerable<TimeEntry>> GetAllAsync()
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Include(te => te.Approver)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TimeEntry>> GetByUserAsync(Guid userId)
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Include(te => te.Approver)
                .Where(te => te.UserId == userId)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TimeEntry>> GetByProjectAsync(Guid projectId)
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Include(te => te.Approver)
                .Where(te => te.ProjectId == projectId)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TimeEntry>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Include(te => te.Approver)
                .Where(te => te.UserId == userId && 
                            te.StartTime.Date >= startDate.Date && 
                            te.StartTime.Date <= endDate.Date)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TimeEntry>> GetByProjectAndDateRangeAsync(Guid projectId, DateTime startDate, DateTime endDate)
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Include(te => te.Approver)
                .Where(te => te.ProjectId == projectId && 
                            te.StartTime.Date >= startDate.Date && 
                            te.StartTime.Date <= endDate.Date)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TimeEntry>> GetPendingApprovalsAsync()
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Where(te => !te.IsApproved && te.EndTime.HasValue)
                .OrderBy(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TimeEntry>> GetActiveTimersByUserAsync(Guid userId)
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Where(te => te.UserId == userId && !te.EndTime.HasValue)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }
        
        public async Task<TimeEntry> CreateAsync(TimeEntry timeEntry)
        {
            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();
            return timeEntry;
        }
        
        public async Task<TimeEntry> UpdateAsync(TimeEntry timeEntry)
        {
            _context.TimeEntries.Update(timeEntry);
            await _context.SaveChangesAsync();
            return timeEntry;
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry != null)
            {
                _context.TimeEntries.Remove(timeEntry);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.TimeEntries.AnyAsync(te => te.Id == id);
        }
        
        public async Task<bool> HasActiveTimerAsync(Guid userId)
        {
            return await _context.TimeEntries
                .AnyAsync(te => te.UserId == userId && !te.EndTime.HasValue);
        }
    }
}
