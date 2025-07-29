using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimesheetTracker.API.DTOs;
using TimesheetTracker.API.Services;

namespace TimesheetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly ILogger<TimeEntriesController> _logger;
        
        public TimeEntriesController(ITimeEntryService timeEntryService, ILogger<TimeEntriesController> logger)
        {
            _timeEntryService = timeEntryService;
            _logger = logger;
        }
        
        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found in token"));
        }
        
        /// <summary>
        /// Get all time entries for the current user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TimeEntryDto>>>> GetMyTimeEntries()
        {
            try
            {
                var userId = GetCurrentUserId();
                var timeEntries = await _timeEntryService.GetByUserAsync(userId);
                return Ok(ApiResponse<IEnumerable<TimeEntryDto>>.SuccessResult(timeEntries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving time entries");
                return StatusCode(500, ApiResponse<IEnumerable<TimeEntryDto>>.ErrorResult("An error occurred while retrieving time entries"));
            }
        }
        
        /// <summary>
        /// Get time entries by date range
        /// </summary>
        [HttpGet("range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TimeEntryDto>>>> GetTimeEntriesByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                var timeEntries = await _timeEntryService.GetByUserAndDateRangeAsync(userId, startDate, endDate);
                return Ok(ApiResponse<IEnumerable<TimeEntryDto>>.SuccessResult(timeEntries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving time entries by date range");
                return StatusCode(500, ApiResponse<IEnumerable<TimeEntryDto>>.ErrorResult("An error occurred while retrieving time entries"));
            }
        }
        
        /// <summary>
        /// Get active timers for the current user
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TimeEntryDto>>>> GetActiveTimers()
        {
            try
            {
                var userId = GetCurrentUserId();
                var activeTimers = await _timeEntryService.GetActiveTimersByUserAsync(userId);
                return Ok(ApiResponse<IEnumerable<TimeEntryDto>>.SuccessResult(activeTimers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active timers");
                return StatusCode(500, ApiResponse<IEnumerable<TimeEntryDto>>.ErrorResult("An error occurred while retrieving active timers"));
            }
        }
        
        /// <summary>
        /// Get a specific time entry by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TimeEntryDto>>> GetTimeEntry(Guid id)
        {
            try
            {
                var timeEntry = await _timeEntryService.GetByIdAsync(id);
                
                if (timeEntry == null)
                {
                    return NotFound(ApiResponse<TimeEntryDto>.ErrorResult("Time entry not found"));
                }
                
                return Ok(ApiResponse<TimeEntryDto>.SuccessResult(timeEntry));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving time entry {TimeEntryId}", id);
                return StatusCode(500, ApiResponse<TimeEntryDto>.ErrorResult("An error occurred while retrieving the time entry"));
            }
        }
        
        /// <summary>
        /// Create a new time entry
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TimeEntryDto>>> CreateTimeEntry([FromBody] CreateTimeEntryDto createTimeEntryDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var timeEntry = await _timeEntryService.CreateAsync(createTimeEntryDto, userId);
                
                return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, 
                    ApiResponse<TimeEntryDto>.SuccessResult(timeEntry, "Time entry created successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating time entry");
                return StatusCode(500, ApiResponse<TimeEntryDto>.ErrorResult("An error occurred while creating the time entry"));
            }
        }
        
        /// <summary>
        /// Start a new timer
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<ApiResponse<TimeEntryDto>>> StartTimer([FromBody] StartTimeEntryDto startTimeEntryDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var timeEntry = await _timeEntryService.StartTimerAsync(startTimeEntryDto, userId);
                
                return Ok(ApiResponse<TimeEntryDto>.SuccessResult(timeEntry, "Timer started successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting timer");
                return StatusCode(500, ApiResponse<TimeEntryDto>.ErrorResult("An error occurred while starting the timer"));
            }
        }
        
        /// <summary>
        /// Stop a timer
        /// </summary>
        [HttpPost("{id}/stop")]
        public async Task<ActionResult<ApiResponse<TimeEntryDto>>> StopTimer(Guid id, [FromBody] StopTimeEntryDto? stopTimeEntryDto = null)
        {
            try
            {
                var endTime = stopTimeEntryDto?.EndTime;
                var timeEntry = await _timeEntryService.StopTimerAsync(id, endTime);
                
                return Ok(ApiResponse<TimeEntryDto>.SuccessResult(timeEntry, "Timer stopped successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping timer {TimeEntryId}", id);
                return StatusCode(500, ApiResponse<TimeEntryDto>.ErrorResult("An error occurred while stopping the timer"));
            }
        }
        
        /// <summary>
        /// Update a time entry
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TimeEntryDto>>> UpdateTimeEntry(Guid id, [FromBody] UpdateTimeEntryDto updateTimeEntryDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var timeEntry = await _timeEntryService.UpdateAsync(id, updateTimeEntryDto, userId);
                
                return Ok(ApiResponse<TimeEntryDto>.SuccessResult(timeEntry, "Time entry updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating time entry {TimeEntryId}", id);
                return StatusCode(500, ApiResponse<TimeEntryDto>.ErrorResult("An error occurred while updating the time entry"));
            }
        }
        
        /// <summary>
        /// Delete a time entry
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTimeEntry(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _timeEntryService.DeleteAsync(id, userId);
                
                return Ok(ApiResponse<object>.SuccessResult(null, "Time entry deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResult(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting time entry {TimeEntryId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResult("An error occurred while deleting the time entry"));
            }
        }
        
        /// <summary>
        /// Approve a time entry (Manager/Admin only)
        /// </summary>
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult<ApiResponse<TimeEntryDto>>> ApproveTimeEntry(Guid id)
        {
            try
            {
                var approverId = GetCurrentUserId();
                var timeEntry = await _timeEntryService.ApproveAsync(id, approverId);
                
                return Ok(ApiResponse<TimeEntryDto>.SuccessResult(timeEntry, "Time entry approved successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<TimeEntryDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving time entry {TimeEntryId}", id);
                return StatusCode(500, ApiResponse<TimeEntryDto>.ErrorResult("An error occurred while approving the time entry"));
            }
        }
        
        /// <summary>
        /// Get pending approvals (Manager/Admin only)
        /// </summary>
        [HttpGet("pending-approvals")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TimeEntryDto>>>> GetPendingApprovals()
        {
            try
            {
                var pendingApprovals = await _timeEntryService.GetPendingApprovalsAsync();
                return Ok(ApiResponse<IEnumerable<TimeEntryDto>>.SuccessResult(pendingApprovals));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending approvals");
                return StatusCode(500, ApiResponse<IEnumerable<TimeEntryDto>>.ErrorResult("An error occurred while retrieving pending approvals"));
            }
        }
    }
}
