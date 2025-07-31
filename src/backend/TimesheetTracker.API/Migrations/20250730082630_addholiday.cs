using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimesheetTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class addholiday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("8ed2ccaa-799f-4ae1-8f26-a47cff54a637"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0745a0e8-ffef-4897-ab1f-b6c9f649efe3"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("a9b886cc-2591-49a4-8216-dd1025ded77c"), new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(3976), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$TrfNUJF5tT6nav2uOxttFO46t7gghP992GuQvg4FLzgtvw76BwL96", "Admin", new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(3981) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("724451a5-3af1-4fc6-87de-5b262f60b0a8"), 10000m, "Internal", new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(4741), new Guid("a9b886cc-2591-49a4-8216-dd1025ded77c"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(4742) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("724451a5-3af1-4fc6-87de-5b262f60b0a8"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a9b886cc-2591-49a4-8216-dd1025ded77c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("0745a0e8-ffef-4897-ab1f-b6c9f649efe3"), new DateTime(2025, 7, 29, 13, 42, 33, 459, DateTimeKind.Utc).AddTicks(9404), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$Aai9kljzfj2NwgNEfItzY.MeV9ZvTv1BrCnxXlTlqQ1K4Caq7JUN2", "Admin", new DateTime(2025, 7, 29, 13, 42, 33, 459, DateTimeKind.Utc).AddTicks(9409) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("8ed2ccaa-799f-4ae1-8f26-a47cff54a637"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 42, 33, 460, DateTimeKind.Utc).AddTicks(163), new Guid("0745a0e8-ffef-4897-ab1f-b6c9f649efe3"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 42, 33, 460, DateTimeKind.Utc).AddTicks(164) });
        }
    }
}
