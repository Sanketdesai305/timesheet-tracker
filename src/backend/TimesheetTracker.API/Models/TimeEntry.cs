using System.ComponentModel.DataAnnotations;

namespace TimesheetTracker.API.Models
{
    public class TimeEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid UserId { get; set; }
        
        public Guid ProjectId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TaskName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        [Range(0, 1440)] // Maximum 24 hours in minutes
        public int Duration { get; set; } // Duration in minutes
        
        public bool IsApproved { get; set; } = false;
        
        public Guid? ApprovedBy { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
        public virtual User? Approver { get; set; }
        
        // Calculated properties
        public decimal HoursWorked => Duration / 60.0m;
        public bool IsCompleted => EndTime.HasValue;
        public TimeSpan? ActualDuration => EndTime.HasValue ? EndTime.Value - StartTime : null;
    }
}
