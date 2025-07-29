using System.ComponentModel.DataAnnotations;

namespace TimesheetTracker.API.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
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
        
        public bool IsActive { get; set; } = true;
        
        public Guid CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
        
        // Calculated properties
        public decimal TotalHoursLogged => TimeEntries.Sum(te => te.Duration) / 60.0m; // Convert minutes to hours
        public decimal TotalCost => TimeEntries.Where(te => te.IsApproved).Sum(te => te.Duration * 1.0m); // Simplified cost calculation
    }
}
