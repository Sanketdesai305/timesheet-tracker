using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimesheetTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class ResetSchma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("0745a0e8-ffef-4897-ab1f-b6c9f649efe3"), new DateTime(2025, 7, 29, 13, 42, 33, 459, DateTimeKind.Utc).AddTicks(9404), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$Aai9kljzfj2NwgNEfItzY.MeV9ZvTv1BrCnxXlTlqQ1K4Caq7JUN2", "Admin", new DateTime(2025, 7, 29, 13, 42, 33, 459, DateTimeKind.Utc).AddTicks(9409) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("8ed2ccaa-799f-4ae1-8f26-a47cff54a637"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 42, 33, 460, DateTimeKind.Utc).AddTicks(163), new Guid("0745a0e8-ffef-4897-ab1f-b6c9f649efe3"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 42, 33, 460, DateTimeKind.Utc).AddTicks(164) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("f28acc2d-397e-49fd-a6ef-6b1dbfd66fff"), new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(3950), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$peu3tiJPIC0iFbdPP1arNu6cl1Wq6uYaTei9bchA5snfbNu0KF.v6", "Admin", new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(3955) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("a8cbf801-9460-4feb-909f-c18e6ea6e1b3"), 10000m, "Internal", new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(4667), new Guid("f28acc2d-397e-49fd-a6ef-6b1dbfd66fff"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 29, 13, 25, 6, 657, DateTimeKind.Utc).AddTicks(4671) });
        }
    }
}
