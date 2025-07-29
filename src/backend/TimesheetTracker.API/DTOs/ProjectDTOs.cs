using System.ComponentModel.DataAnnotations;

namespace TimesheetTracker.API.DTOs
{
    // Project DTOs
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ClientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal TotalHoursLogged { get; set; }
        public int TimeEntriesCount { get; set; }
    }
    
    public class CreateProjectDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(200)]
        public string? ClientName { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? Budget { get; set; }
    }
    
    public class UpdateProjectDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(200)]
        public string? ClientName { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? Budget { get; set; }
        
        public bool IsActive { get; set; }
    }
    
    // Report DTOs
    public class ReportRequestDto
    {
        public Guid? UserId { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool OnlyApproved { get; set; } = false;
    }
    
    public class TimeReportDto
    {
        public string Period { get; set; } = string.Empty;
        public List<TimeEntryDto> TimeEntries { get; set; } = new();
        public decimal TotalHours { get; set; }
        public decimal TotalApprovedHours { get; set; }
        public int TotalEntries { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
    
    public class ProjectReportDto
    {
        public ProjectDto Project { get; set; } = null!;
        public List<UserTimeDto> UserTimeBreakdown { get; set; } = new();
        public decimal TotalProjectHours { get; set; }
        public decimal BudgetUtilization { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
    
    public class UserTimeDto
    {
        public UserDto User { get; set; } = null!;
        public decimal TotalHours { get; set; }
        public int TotalEntries { get; set; }
    }
    
    // Common response wrapper
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        
        public static ApiResponse<T> SuccessResult(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        
        public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
