using AutoMapper;
using TimesheetTracker.API.DTOs;
using TimesheetTracker.API.Models;
using TimesheetTracker.API.Repositories;

namespace TimesheetTracker.API.Services
{
    public class ProjectService : IProjectService
    {

        public async Task<IEnumerable<ProjectDto>> GetActiveProjectsByUserAsync(Guid userId)
        {
            var projects = await _projectRepository.GetActiveProjectsByUserAsync(userId);
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        
        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        
        public async Task<ProjectDto?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return project != null ? _mapper.Map<ProjectDto>(project) : null;
        }
        
        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
        
        public async Task<IEnumerable<ProjectDto>> GetByUserAsync(Guid userId)
        {
            var projects = await _projectRepository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
        
        public async Task<IEnumerable<ProjectDto>> GetActiveProjectsAsync()
        {
            var projects = await _projectRepository.GetActiveProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
        
        public async Task<ProjectDto> CreateAsync(CreateProjectDto createProjectDto, Guid createdBy)
        {
            var project = _mapper.Map<Project>(createProjectDto);
            project.CreatedBy = createdBy;
            
            await _projectRepository.CreateAsync(project);
            
            // Reload with navigation properties
            var createdProject = await _projectRepository.GetByIdAsync(project.Id);
            return _mapper.Map<ProjectDto>(createdProject);
        }
        
        public async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto updateProjectDto)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                throw new KeyNotFoundException("Project not found");
            }
            
            _mapper.Map(updateProjectDto, existingProject);
            await _projectRepository.UpdateAsync(existingProject);
            
            return _mapper.Map<ProjectDto>(existingProject);
        }
        
        public async Task DeleteAsync(Guid id)
        {
            if (!await _projectRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException("Project not found");
            }
            
            await _projectRepository.DeleteAsync(id);
        }
        
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _projectRepository.ExistsAsync(id);
        }
    }
    
    public class TimeEntryService : ITimeEntryService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public TimeEntryService(
            ITimeEntryRepository timeEntryRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _timeEntryRepository = timeEntryRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<TimeEntryDto?> GetByIdAsync(Guid id)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            return timeEntry != null ? _mapper.Map<TimeEntryDto>(timeEntry) : null;
        }
        
        public async Task<IEnumerable<TimeEntryDto>> GetAllAsync()
        {
            var timeEntries = await _timeEntryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries);
        }
        
        public async Task<IEnumerable<TimeEntryDto>> GetByUserAsync(Guid userId)
        {
            var timeEntries = await _timeEntryRepository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries);
        }
        
        public async Task<IEnumerable<TimeEntryDto>> GetByProjectAsync(Guid projectId)
        {
            var timeEntries = await _timeEntryRepository.GetByProjectAsync(projectId);
            return _mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries);
        }
        
        public async Task<IEnumerable<TimeEntryDto>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var timeEntries = await _timeEntryRepository.GetByUserAndDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries);
        }
        
        public async Task<IEnumerable<TimeEntryDto>> GetPendingApprovalsAsync()
        {
            var timeEntries = await _timeEntryRepository.GetPendingApprovalsAsync();
            return _mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries);
        }
        
        public async Task<IEnumerable<TimeEntryDto>> GetActiveTimersByUserAsync(Guid userId)
        {
            var timeEntries = await _timeEntryRepository.GetActiveTimersByUserAsync(userId);
            return _mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries);
        }
        
        public async Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto createTimeEntryDto, Guid userId)
        {
            // Validate project exists
            if (!await _projectRepository.ExistsAsync(createTimeEntryDto.ProjectId))
            {
                throw new KeyNotFoundException("Project not found");
            }
            
            var timeEntry = _mapper.Map<TimeEntry>(createTimeEntryDto);
            timeEntry.UserId = userId;
            
            // Calculate duration if both start and end times are provided
            if (timeEntry.EndTime.HasValue)
            {
                var duration = (int)(timeEntry.EndTime.Value - timeEntry.StartTime).TotalMinutes;
                timeEntry.Duration = Math.Max(0, duration);
            }
            
            await _timeEntryRepository.CreateAsync(timeEntry);
            
            // Reload with navigation properties
            var createdTimeEntry = await _timeEntryRepository.GetByIdAsync(timeEntry.Id);
            return _mapper.Map<TimeEntryDto>(createdTimeEntry);
        }
        
        public async Task<TimeEntryDto> StartTimerAsync(StartTimeEntryDto startTimeEntryDto, Guid userId)
        {
            // Check if user already has an active timer
            if (await _timeEntryRepository.HasActiveTimerAsync(userId))
            {
                throw new InvalidOperationException("User already has an active timer running");
            }
            
            // Validate project exists
            if (!await _projectRepository.ExistsAsync(startTimeEntryDto.ProjectId))
            {
                throw new KeyNotFoundException("Project not found");
            }
            
            var timeEntry = _mapper.Map<TimeEntry>(startTimeEntryDto);
            timeEntry.UserId = userId;
            timeEntry.StartTime = DateTime.UtcNow;
            
            await _timeEntryRepository.CreateAsync(timeEntry);
            
            // Reload with navigation properties
            var createdTimeEntry = await _timeEntryRepository.GetByIdAsync(timeEntry.Id);
            return _mapper.Map<TimeEntryDto>(createdTimeEntry);
        }
        
        public async Task<TimeEntryDto> StopTimerAsync(Guid timeEntryId, DateTime? endTime = null)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(timeEntryId);
            if (timeEntry == null)
            {
                throw new KeyNotFoundException("Time entry not found");
            }
            
            if (timeEntry.EndTime.HasValue)
            {
                throw new InvalidOperationException("Timer is already stopped");
            }
            
            timeEntry.EndTime = endTime ?? DateTime.UtcNow;
            timeEntry.Duration = (int)(timeEntry.EndTime.Value - timeEntry.StartTime).TotalMinutes;
            
            await _timeEntryRepository.UpdateAsync(timeEntry);
            return _mapper.Map<TimeEntryDto>(timeEntry);
        }
        
        public async Task<TimeEntryDto> UpdateAsync(Guid id, UpdateTimeEntryDto updateTimeEntryDto, Guid userId)
        {
            var existingTimeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (existingTimeEntry == null)
            {
                throw new KeyNotFoundException("Time entry not found");
            }
            
            // Users can only edit their own time entries (unless they're managers/admins)
            if (existingTimeEntry.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only edit your own time entries");
            }
            
            // Validate project exists
            if (!await _projectRepository.ExistsAsync(updateTimeEntryDto.ProjectId))
            {
                throw new KeyNotFoundException("Project not found");
            }
            
            _mapper.Map(updateTimeEntryDto, existingTimeEntry);
            
            // Recalculate duration if both times are provided
            if (existingTimeEntry.EndTime.HasValue)
            {
                var duration = (int)(existingTimeEntry.EndTime.Value - existingTimeEntry.StartTime).TotalMinutes;
                existingTimeEntry.Duration = Math.Max(0, duration);
            }
            
            await _timeEntryRepository.UpdateAsync(existingTimeEntry);
            return _mapper.Map<TimeEntryDto>(existingTimeEntry);
        }
        
        public async Task<TimeEntryDto> ApproveAsync(Guid id, Guid approverId)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
            {
                throw new KeyNotFoundException("Time entry not found");
            }
            
            if (timeEntry.IsApproved)
            {
                throw new InvalidOperationException("Time entry is already approved");
            }
            
            if (!timeEntry.EndTime.HasValue)
            {
                throw new InvalidOperationException("Cannot approve an incomplete time entry");
            }
            
            timeEntry.IsApproved = true;
            timeEntry.ApprovedBy = approverId;
            timeEntry.ApprovedAt = DateTime.UtcNow;
            
            await _timeEntryRepository.UpdateAsync(timeEntry);
            return _mapper.Map<TimeEntryDto>(timeEntry);
        }
        
        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
            {
                throw new KeyNotFoundException("Time entry not found");
            }
            
            // Users can only delete their own time entries (unless they're managers/admins)
            if (timeEntry.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own time entries");
            }
            
            await _timeEntryRepository.DeleteAsync(id);
        }
        
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _timeEntryRepository.ExistsAsync(id);
        }
        
        public async Task<bool> HasActiveTimerAsync(Guid userId)
        {
            return await _timeEntryRepository.HasActiveTimerAsync(userId);
        }
    }
    
    public class ReportService : IReportService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public ReportService(
            ITimeEntryRepository timeEntryRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _timeEntryRepository = timeEntryRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<TimeReportDto> GenerateUserTimeReportAsync(Guid userId, DateTime startDate, DateTime endDate, bool onlyApproved = false)
        {
            var timeEntries = await _timeEntryRepository.GetByUserAndDateRangeAsync(userId, startDate, endDate);
            
            if (onlyApproved)
            {
                timeEntries = timeEntries.Where(te => te.IsApproved);
            }
            
            var timeEntryDtos = _mapper.Map<List<TimeEntryDto>>(timeEntries);
            
            return new TimeReportDto
            {
                Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                TimeEntries = timeEntryDtos,
                TotalHours = timeEntryDtos.Sum(te => te.HoursWorked),
                TotalApprovedHours = timeEntryDtos.Where(te => te.IsApproved).Sum(te => te.HoursWorked),
                TotalEntries = timeEntryDtos.Count
            };
        }
        
        public async Task<ProjectReportDto> GenerateProjectReportAsync(Guid projectId, DateTime startDate, DateTime endDate)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }
            
            var timeEntries = await _timeEntryRepository.GetByProjectAndDateRangeAsync(projectId, startDate, endDate);
            var userTimeBreakdown = timeEntries
                .GroupBy(te => te.User)
                .Select(g => new UserTimeDto
                {
                    User = _mapper.Map<UserDto>(g.Key),
                    TotalHours = g.Sum(te => te.Duration) / 60.0m,
                    TotalEntries = g.Count()
                })
                .ToList();
            
            var totalHours = timeEntries.Sum(te => te.Duration) / 60.0m;
            var budgetUtilization = project.Budget.HasValue && project.Budget > 0 
                ? (totalHours / project.Budget.Value) * 100 
                : 0;
            
            return new ProjectReportDto
            {
                Project = _mapper.Map<ProjectDto>(project),
                UserTimeBreakdown = userTimeBreakdown,
                TotalProjectHours = totalHours,
                BudgetUtilization = budgetUtilization
            };
        }
        
        public async Task<TimeReportDto> GenerateTeamTimeReportAsync(DateTime startDate, DateTime endDate, bool onlyApproved = false)
        {
            var allTimeEntries = await _timeEntryRepository.GetAllAsync();
            var filteredEntries = allTimeEntries.Where(te => 
                te.StartTime.Date >= startDate.Date && 
                te.StartTime.Date <= endDate.Date);
            
            if (onlyApproved)
            {
                filteredEntries = filteredEntries.Where(te => te.IsApproved);
            }
            
            var timeEntryDtos = _mapper.Map<List<TimeEntryDto>>(filteredEntries);
            
            return new TimeReportDto
            {
                Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                TimeEntries = timeEntryDtos,
                TotalHours = timeEntryDtos.Sum(te => te.HoursWorked),
                TotalApprovedHours = timeEntryDtos.Where(te => te.IsApproved).Sum(te => te.HoursWorked),
                TotalEntries = timeEntryDtos.Count
            };
        }
        
        public async Task<byte[]> ExportToExcelAsync(TimeReportDto report)
        {
            // Placeholder implementation - would use a library like EPPlus or ClosedXML
            var csv = "Date,Project,Task,Hours,Status\n";
            foreach (var entry in report.TimeEntries)
            {
                csv += $"{entry.StartTime:yyyy-MM-dd},{entry.ProjectName},{entry.TaskName},{entry.HoursWorked},{(entry.IsApproved ? "Approved" : "Pending")}\n";
            }
            
            return System.Text.Encoding.UTF8.GetBytes(csv);
        }
        
        public async Task<byte[]> ExportToPdfAsync(TimeReportDto report)
        {
            // Placeholder implementation - would use a library like iTextSharp or PdfSharp
            var html = $"<h1>Time Report</h1><p>Period: {report.Period}</p><p>Total Hours: {report.TotalHours}</p>";
            return System.Text.Encoding.UTF8.GetBytes(html);
        }
    }
}
