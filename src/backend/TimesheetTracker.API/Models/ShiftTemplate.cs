using System;
using System.Collections.Generic;

namespace TimesheetTracker.API.Models
{
    public class ShiftTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RecurrenceRule { get; set; } = string.Empty; // e.g., "FREQ=WEEKLY;BYDAY=MO,WE,FR"
        public bool IsActive { get; set; } = true;
        public List<ShiftInstance> Instances { get; set; } = new();
    }
}
