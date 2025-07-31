using System.ComponentModel.DataAnnotations;

namespace TimesheetTracker.Client.Models
{
    // Authentication DTOs
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public UserDto User { get; set; } = new();
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    // User DTOs
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Employee, TeamLead, Manager, Admin, FinanceHR
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Employee"; // Employee, TeamLead, Manager, Admin, FinanceHR
    }

    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Employee, TeamLead, Manager, Admin, FinanceHR
        public bool IsActive { get; set; }
    }

    // Project DTOs
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ClientName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Budget { get; set; }
    }

    public class UpdateProjectRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public string ClientName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

    // Time Entry DTOs
    public class TimeEntryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string TaskName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovalComments { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTimeEntryRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }

    public class UpdateTimeEntryRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }

    public class StartTimerRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string TaskName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public class ApproveTimeEntryRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        public string? Comments { get; set; }
    }

    // Report DTOs
    public class TimeEntryReportDto
    {
        public string UserName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Duration { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class UserTimeReportDto
    {
        public string UserName { get; set; } = string.Empty;
        public double TotalHours { get; set; }
        public double ApprovedHours { get; set; }
        public double PendingHours { get; set; }
        public double RejectedHours { get; set; }
    }

    public class ProjectTimeReportDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public double TotalHours { get; set; }
        public double ApprovedHours { get; set; }
        public double PendingHours { get; set; }
        public double RejectedHours { get; set; }
    }
}
