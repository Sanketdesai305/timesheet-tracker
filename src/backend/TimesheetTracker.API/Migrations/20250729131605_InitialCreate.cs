using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimesheetTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("de0277d1-9d2c-474a-a5d3-fac33e18a507"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ddb08eaf-a9b9-4ee3-9b4e-9f386406939d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("743bd8d2-56db-49a7-9a38-448343a19c7c"), new DateTime(2025, 7, 29, 13, 16, 4, 866, DateTimeKind.Utc).AddTicks(8915), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$XRH0qNLI1E32okHJHLBhNOFVcjWH0lkCouRGE22DN/1tbziKt9JR6", "Admin", new DateTime(2025, 7, 29, 13, 16, 4, 866, DateTimeKind.Utc).AddTicks(8922) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("3235253c-db35-4c03-b4a0-53b2440b0863"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 16, 4, 867, DateTimeKind.Utc).AddTicks(222), new Guid("743bd8d2-56db-49a7-9a38-448343a19c7c"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 16, 4, 867, DateTimeKind.Utc).AddTicks(223) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("ddb08eaf-a9b9-4ee3-9b4e-9f386406939d"), new DateTime(2025, 7, 29, 13, 2, 15, 701, DateTimeKind.Utc).AddTicks(6229), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$/C4fYJeSKV54F8ZD2Daps.QgBPAcmYWCR6kkETacVOdmlwvKMcgoy", "Admin", new DateTime(2025, 7, 29, 13, 2, 15, 701, DateTimeKind.Utc).AddTicks(6236) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("de0277d1-9d2c-474a-a5d3-fac33e18a507"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 2, 15, 701, DateTimeKind.Utc).AddTicks(7456), new Guid("ddb08eaf-a9b9-4ee3-9b4e-9f386406939d"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 2, 15, 701, DateTimeKind.Utc).AddTicks(7457) });
        }
    }
}
