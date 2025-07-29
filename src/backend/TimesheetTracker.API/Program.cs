using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TimesheetTracker.API.Data;
using TimesheetTracker.API.Services;
using TimesheetTracker.API.Repositories;
using FluentValidation;
using AutoMapper;
using TimesheetTracker.API.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/timesheet-tracker-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Timesheet Tracker API", 
        Version = "v1",
        Description = "A comprehensive timesheet tracking system"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Add Entity Framework
builder.Services.AddDbContext<TimesheetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("https://localhost:7001", "http://localhost:5000", "http://localhost:5044", "https://localhost:5044")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timesheet Tracker API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created and updated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TimesheetDbContext>();
    context.Database.EnsureCreated();
    
    // Add Department column if it doesn't exist
    try
    {
        var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();
        
        var checkColumnSql = @"
            SELECT COUNT(*) 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'Department'";
        
        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = checkColumnSql;
        var columnExists = (int)(await checkCommand.ExecuteScalarAsync() ?? 0);
        
        if (columnExists == 0)
        {
            var addColumnSql = "ALTER TABLE Users ADD Department NVARCHAR(100) NOT NULL DEFAULT ''";
            using var addCommand = connection.CreateCommand();
            addCommand.CommandText = addColumnSql;
            await addCommand.ExecuteNonQueryAsync();
            Log.Information("Department column added to Users table");
        }
        
        await connection.CloseAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error adding Department column");
    }
    
    // Ensure at least one project exists for testing
    try
    {
        var projectCount = await context.Projects.CountAsync();
        if (projectCount == 0)
        {
            // Get or create an admin user for the project
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@timesheettracker.com");
            if (adminUser == null)
            {
                adminUser = new TimesheetTracker.API.Models.User
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@timesheettracker.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = TimesheetTracker.API.Models.UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Department = "IT"
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
            
            // Create sample projects
            var sampleProjects = new[]
            {
                new TimesheetTracker.API.Models.Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Website Development",
                    Description = "Main company website development and maintenance",
                    ClientName = "Internal",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddDays(90),
                    Budget = 15000,
                    IsActive = true,
                    CreatedBy = adminUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TimesheetTracker.API.Models.Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Mobile App",
                    Description = "Customer mobile application development",
                    ClientName = "ABC Corp",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddDays(120),
                    Budget = 25000,
                    IsActive = true,
                    CreatedBy = adminUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TimesheetTracker.API.Models.Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Database Migration",
                    Description = "Legacy system database migration to cloud",
                    ClientName = "XYZ Ltd",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddDays(60),
                    Budget = 8000,
                    IsActive = true,
                    CreatedBy = adminUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            
            context.Projects.AddRange(sampleProjects);
            await context.SaveChangesAsync();
            Log.Information("Sample projects created");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error creating sample projects");
    }
}

app.Run();
