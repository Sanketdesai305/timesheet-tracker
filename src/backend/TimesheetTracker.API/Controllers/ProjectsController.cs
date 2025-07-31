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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;
        
        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }
        
        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found in token"));
        }
        
        /// <summary>
        /// Get active projects for the current user
        /// </summary>
        [HttpGet("active/user")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetActiveProjectsForUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                var projects = await _projectService.GetActiveProjectsByUserAsync(userId);
                return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projects));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active projects for user");
                return StatusCode(500, ApiResponse<IEnumerable<ProjectDto>>.ErrorResult("An error occurred while retrieving active projects for user"));
            }
        }
        /// Get all active projects
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetActiveProjects()
        {
            try
            {
                var projects = await _projectService.GetActiveProjectsAsync();
                return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projects));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active projects");
                return StatusCode(500, ApiResponse<IEnumerable<ProjectDto>>.ErrorResult("An error occurred while retrieving active projects"));
            }
        }
        
        /// <summary>
        /// Get all projects
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetAllProjects()
        {
            try
            {
                var projects = await _projectService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projects));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects");
                return StatusCode(500, ApiResponse<IEnumerable<ProjectDto>>.ErrorResult("An error occurred while retrieving projects"));
            }
        }
        
        /// <summary>
        /// Get a specific project by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(Guid id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);
                
                if (project == null)
                {
                    return NotFound(ApiResponse<ProjectDto>.ErrorResult("Project not found"));
                }
                
                return Ok(ApiResponse<ProjectDto>.SuccessResult(project));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project {ProjectId}", id);
                return StatusCode(500, ApiResponse<ProjectDto>.ErrorResult("An error occurred while retrieving the project"));
            }
        }
        
        /// <summary>
        /// Create a new project
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            try
            {
                _logger.LogInformation("CreateProject called with: {@CreateProjectDto}", createProjectDto);
                var userId = GetCurrentUserId();
                var project = await _projectService.CreateAsync(createProjectDto, userId);
                _logger.LogInformation("Project created successfully: {@Project}", project);
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, ApiResponse<ProjectDto>.SuccessResult(project));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project. Incoming DTO: {@CreateProjectDto}", createProjectDto);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException, "Inner exception during project creation");
                }
                return StatusCode(500, ApiResponse<ProjectDto>.ErrorResult($"An error occurred while creating the project: {ex.Message}"));
            }
        }
        
        /// <summary>
        /// Update an existing project
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(Guid id, [FromBody] UpdateProjectDto updateProjectDto)
        {
            try
            {
                var project = await _projectService.UpdateAsync(id, updateProjectDto);
                
                if (project == null)
                {
                    return NotFound(ApiResponse<ProjectDto>.ErrorResult("Project not found"));
                }
                
                return Ok(ApiResponse<ProjectDto>.SuccessResult(project));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId}", id);
                return StatusCode(500, ApiResponse<ProjectDto>.ErrorResult("An error occurred while updating the project"));
            }
        }
        
        /// <summary>
        /// Delete a project
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProject(Guid id)
        {
            try
            {
                await _projectService.DeleteAsync(id);
                return Ok(ApiResponse<object>.SuccessResult("Project deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResult("An error occurred while deleting the project"));
            }
        }
    }
}
