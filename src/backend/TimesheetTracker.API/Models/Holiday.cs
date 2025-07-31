using System;

namespace TimesheetTracker.API.Models
{
    public class Holiday
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsPublic { get; set; } = true;
        public string? Country { get; set; }
    }
}
