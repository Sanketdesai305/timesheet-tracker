using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimesheetTracker.API.DTOs;
using TimesheetTracker.API.Services;

namespace TimesheetTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        
        /// <summary>
        /// Authenticate user and return JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var result = await _authService.LoginAsync(loginRequest);
                
                if (result == null)
                {
                    return Unauthorized(ApiResponse<AuthResponseDto>.ErrorResult("Invalid email or password"));
                }
                
                _logger.LogInformation("User {Email} logged in successfully", loginRequest.Email);
                return Ok(ApiResponse<AuthResponseDto>.SuccessResult(result, "Login successful"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Email}", loginRequest.Email);
                return StatusCode(500, ApiResponse<AuthResponseDto>.ErrorResult("An error occurred during login"));
            }
        }
        
        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerRequest);
                
                _logger.LogInformation("User {Email} registered successfully", registerRequest.Email);
                return CreatedAtAction(nameof(Register), ApiResponse<AuthResponseDto>.SuccessResult(result, "Registration successful"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<AuthResponseDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", registerRequest.Email);
                return StatusCode(500, ApiResponse<AuthResponseDto>.ErrorResult("An error occurred during registration"));
            }
        }
        
        /// <summary>
        /// Get current user information
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public ActionResult<ApiResponse<object>> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            var currentUser = new
            {
                Id = userId,
                Email = email,
                Name = name,
                Role = role
            };
            
            return Ok(ApiResponse<object>.SuccessResult(currentUser));
        }
        
        /// <summary>
        /// Logout user (client-side token invalidation)
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public ActionResult<ApiResponse<object>> Logout()
        {
            // In a real application, you might want to invalidate the token on the server side
            // For now, we'll just return a success response as the client will discard the token
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("User {UserId} logged out", userId);
            
            return Ok(ApiResponse<object>.SuccessResult(null, "Logout successful"));
        }
    }
}
