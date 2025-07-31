using System;
using System.Collections.Generic;

namespace TimesheetTracker.Client.Models
{
    public class ShiftTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RecurrenceRule { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<ShiftInstanceDto> Instances { get; set; } = new();
    }
}
