using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimesheetTracker.API.DTOs;
using TimesheetTracker.API.Models;
using TimesheetTracker.API.Repositories;

namespace TimesheetTracker.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        
        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
            
            if (user == null || !user.IsActive || !await ValidatePasswordAsync(loginRequest.Password, user.PasswordHash))
            {
                return null;
            }
            
            var token = await GenerateJwtTokenAsync(user);
            
            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = Guid.NewGuid().ToString(), // Simplified refresh token
                Expires = DateTime.UtcNow.AddHours(24),
                User = _mapper.Map<UserDto>(user)
            };
        }
        
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            
            // Create new user
            var user = new User
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Department = registerRequest.Department ?? string.Empty,
                PasswordHash = HashPassword(registerRequest.Password),
                Role = UserRole.Employee,
                IsActive = true
            };
            
            await _userRepository.CreateAsync(user);
            
            var token = await GenerateJwtTokenAsync(user);
            
            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddHours(24),
                User = _mapper.Map<UserDto>(user)
            };
        }
        
        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };
            
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public async Task<bool> ValidatePasswordAsync(string password, string hashedPassword)
        {
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hashedPassword));
        }
        
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
    
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        
        public UserService(IUserRepository userRepository, IMapper mapper, IAuthService authService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
        }
        
        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }
        
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        
        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(createUserDto.Email))
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            
            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = _authService.HashPassword(createUserDto.Password);
            
            await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(user);
        }
        
        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            
            // Check if email is being changed and if it already exists
            if (existingUser.Email != updateUserDto.Email && 
                await _userRepository.EmailExistsAsync(updateUserDto.Email, id))
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            
            _mapper.Map(updateUserDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
            
            return _mapper.Map<UserDto>(existingUser);
        }
        
        public async Task DeleteAsync(Guid id)
        {
            if (!await _userRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException("User not found");
            }
            
            await _userRepository.DeleteAsync(id);
        }
        
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _userRepository.ExistsAsync(id);
        }
    }
}
