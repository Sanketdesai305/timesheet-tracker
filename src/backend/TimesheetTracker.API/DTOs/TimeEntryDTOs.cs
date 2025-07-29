using System.ComponentModel.DataAnnotations;

namespace TimesheetTracker.API.DTOs
{
    // TimeEntry DTOs
    public class TimeEntryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string TaskName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }
        public decimal HoursWorked { get; set; }
        public bool IsApproved { get; set; }
        public string? ApproverName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
    
    public class CreateTimeEntryDto
    {
        [Required]
        public Guid ProjectId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TaskName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        [Range(0, 1440)]
        public int? Duration { get; set; }
    }
    
    public class UpdateTimeEntryDto
    {
        [Required]
        public Guid ProjectId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TaskName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        [Range(0, 1440)]
        public int Duration { get; set; }
    }
    
    public class StartTimeEntryDto
    {
        [Required]
        public Guid ProjectId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TaskName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
    }
    
    public class StopTimeEntryDto
    {
        [Required]
        public Guid TimeEntryId { get; set; }
        
        public DateTime? EndTime { get; set; }
    }
}
