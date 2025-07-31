using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimesheetTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class ResetSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("3235253c-db35-4c03-b4a0-53b2440b0863"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("743bd8d2-56db-49a7-9a38-448343a19c7c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("f28acc2d-397e-49fd-a6ef-6b1dbfd66fff"), new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(3950), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$peu3tiJPIC0iFbdPP1arNu6cl1Wq6uYaTei9bchA5snfbNu0KF.v6", "Admin", new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(3955) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("a8cbf801-9460-4feb-909f-c18e6ea6e1b3"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(4667), new Guid("f28acc2d-397e-49fd-a6ef-6b1dbfd66fff"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(4671) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("a8cbf801-9460-4feb-909f-c18e6ea6e1b3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f28acc2d-397e-49fd-a6ef-6b1dbfd66fff"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("743bd8d2-56db-49a7-9a38-448343a19c7c"), new DateTime(2025, 7, 29, 13, 16, 4, 866, DateTimeKind.Utc).AddTicks(8915), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$XRH0qNLI1E32okHJHLBhNOFVcjWH0lkCouRGE22DN/1tbziKt9JR6", "Admin", new DateTime(2025, 7, 29, 13, 16, 4, 866, DateTimeKind.Utc).AddTicks(8922) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("3235253c-db35-4c03-b4a0-53b2440b0863"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 16, 4, 867, DateTimeKind.Utc).AddTicks(222), new Guid("743bd8d2-56db-49a7-9a38-448343a19c7c"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 16, 4, 867, DateTimeKind.Utc).AddTicks(223) });
        }
    }
}
