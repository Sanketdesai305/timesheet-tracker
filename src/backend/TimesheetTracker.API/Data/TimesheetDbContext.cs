using Microsoft.EntityFrameworkCore;
using TimesheetTracker.API.Models;

namespace TimesheetTracker.API.Data
{
    public class TimesheetDbContext : DbContext
    {
        public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        public DbSet<ShiftTemplate> ShiftTemplates { get; set; }
        public DbSet<ShiftInstance> ShiftInstances { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).HasConversion<string>();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
            
            // Project configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.ClientName).HasMaxLength(200);
                entity.Property(e => e.Budget).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasOne(e => e.Creator)
                      .WithMany(u => u.Projects)
                      .HasForeignKey(e => e.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // TimeEntry configuration
            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TaskName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasOne(e => e.User)
                      .WithMany(u => u.TimeEntries)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict
                      
                entity.HasOne(e => e.Project)
                      .WithMany(p => p.TimeEntries)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Approver)
                      .WithMany(u => u.ApprovedTimeEntries)
                      .HasForeignKey(e => e.ApprovedBy)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasIndex(e => new { e.UserId, e.StartTime })
                      .HasDatabaseName("IX_TimeEntries_UserId_StartTime");
                entity.HasIndex(e => e.ProjectId)
                      .HasDatabaseName("IX_TimeEntries_ProjectId");
            });
            
            // Seed data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user
            var adminUserId = Guid.NewGuid();
            var teamLeadId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var financeHrId = Guid.NewGuid();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    Email = "admin@timesheettracker.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = teamLeadId,
                    Email = "teamlead@timesheettracker.com",
                    FirstName = "Team",
                    LastName = "Lead",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("TeamLead@123"),
                    Role = UserRole.TeamLead,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = managerId,
                    Email = "manager@timesheettracker.com",
                    FirstName = "Project",
                    LastName = "Manager",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                    Role = UserRole.Manager,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = financeHrId,
                    Email = "financehr@timesheettracker.com",
                    FirstName = "Finance",
                    LastName = "HR",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("FinanceHR@123"),
                    Role = UserRole.FinanceHR,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
            
            // Seed sample project
            var sampleProjectId = Guid.NewGuid();
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = sampleProjectId,
                    Name = "Sample Project",
                    Description = "This is a sample project for demonstration purposes",
                    ClientName = "Internal",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddDays(30),
                    Budget = 10000,
                    IsActive = true,
                    CreatedBy = adminUserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
        
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
        
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is User ||
                    e.Entity is Project ||
                    e.Entity is TimeEntry ||
                    e.Entity is ShiftTemplate ||
                    e.Entity is ShiftInstance ||
                    e.Entity is Holiday)
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
