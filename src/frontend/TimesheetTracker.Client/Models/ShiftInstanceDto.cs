using System;

namespace TimesheetTracker.Client.Models
{
    public class ShiftInstanceDto
    {
        public Guid Id { get; set; }
        public Guid ShiftTemplateId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AssignedToUserId { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
    }
}
