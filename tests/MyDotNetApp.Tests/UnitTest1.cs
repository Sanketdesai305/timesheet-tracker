using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TimesheetTracker.API.Data;
using TimesheetTracker.API.DTOs;
using TimesheetTracker.API.Models;

namespace MyDotNetApp.Tests
{
    [TestClass]
    public class TimesheetTrackerTests
    {
        private WebApplicationFactory<Program>? _factory;
        private HttpClient? _client;
        
        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the existing DbContext registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<TimesheetDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        
                        // Add in-memory database for testing
                        services.AddDbContext<TimesheetDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDatabase");
                        });
                    });
                });
            
            _client = _factory.CreateClient();
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }
        
        [TestMethod]
        public async Task Register_ShouldCreateNewUser()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };
            
            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            // Act
            var response = await _client!.PostAsync("/api/auth/register", content);
            
            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("test@example.com", result.Data.User.Email);
        }
        
        [TestMethod]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            // First register a user
            await RegisterTestUser();
            
            var loginRequest = new LoginRequestDto
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            // Act
            var response = await _client!.PostAsync("/api/auth/login", content);
            
            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Token);
        }
        
        [TestMethod]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "nonexistent@example.com",
                Password = "WrongPassword"
            };
            
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            // Act
            var response = await _client!.PostAsync("/api/auth/login", content);
            
            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [TestMethod]
        public void User_ShouldCalculateFullNameCorrectly()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe"
            };
            
            // Act
            var fullName = user.FullName;
            
            // Assert
            Assert.AreEqual("John Doe", fullName);
        }
        
        [TestMethod]
        public void TimeEntry_ShouldCalculateHoursWorkedCorrectly()
        {
            // Arrange
            var timeEntry = new TimeEntry
            {
                Duration = 120 // 2 hours in minutes
            };
            
            // Act
            var hoursWorked = timeEntry.HoursWorked;
            
            // Assert
            Assert.AreEqual(2.0m, hoursWorked);
        }
        
        [TestMethod]
        public void TimeEntry_ShouldIdentifyCompletedStatus()
        {
            // Arrange
            var completedEntry = new TimeEntry
            {
                StartTime = DateTime.UtcNow.AddHours(-2),
                EndTime = DateTime.UtcNow
            };
            
            var incompleteEntry = new TimeEntry
            {
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = null
            };
            
            // Act & Assert
            Assert.IsTrue(completedEntry.IsCompleted);
            Assert.IsFalse(incompleteEntry.IsCompleted);
        }
        
        [TestMethod]
        public void Project_ShouldCalculateTotalHoursLogged()
        {
            // Arrange
            var project = new Project
            {
                TimeEntries = new List<TimeEntry>
                {
                    new TimeEntry { Duration = 60 }, // 1 hour
                    new TimeEntry { Duration = 120 }, // 2 hours
                    new TimeEntry { Duration = 90 } // 1.5 hours
                }
            };
            
            // Act
            var totalHours = project.TotalHoursLogged;
            
            // Assert
            Assert.AreEqual(4.5m, totalHours); // 4.5 hours total
        }
        
        private async Task RegisterTestUser()
        {
            var registerRequest = new RegisterRequestDto
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };
            
            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            await _client!.PostAsync("/api/auth/register", content);
        }
    }
}