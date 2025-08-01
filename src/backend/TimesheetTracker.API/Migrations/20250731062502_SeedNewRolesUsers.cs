using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TimesheetTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewRolesUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[,]
                {
                    { new Guid("4171f0df-44bb-44af-835e-dc6916616b39"), new DateTime(2025, 7, 31, 6, 25, 2, 34, DateTimeKind.Utc).AddTicks(5966), "", "manager@timesheettracker.com", "Project", true, "Manager", "$2a$11$wS56TV8bJY89A6h4V9qXEekkMmTBHmtAusqc.og.KkWMyDwRIkm/e", "Manager", new DateTime(2025, 7, 31, 6, 25, 2, 34, DateTimeKind.Utc).AddTicks(5972) },
                    { new Guid("6299a0b7-e6cd-453d-bb48-246439c40724"), new DateTime(2025, 7, 31, 6, 25, 1, 759, DateTimeKind.Utc).AddTicks(7564), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$8kRzDIB59NsDcefeKFTWauMvKDv29YzrHd8YtNbhx3kDf8zuBUnjC", "Admin", new DateTime(2025, 7, 31, 6, 25, 1, 759, DateTimeKind.Utc).AddTicks(7573) },
                    { new Guid("8a59ff26-bd9a-4c83-bfc2-92f8dd10a311"), new DateTime(2025, 7, 31, 6, 25, 1, 896, DateTimeKind.Utc).AddTicks(5283), "", "teamlead@timesheettracker.com", "Team", true, "Lead", "$2a$11$3CNzJp6.sNApHT4Vpw8h1eTyB/C8baWggxCDj7BPUKDTgUyXyuMIS", "TeamLead", new DateTime(2025, 7, 31, 6, 25, 1, 896, DateTimeKind.Utc).AddTicks(5289) },
                    { new Guid("dbf0c851-44e1-409b-87aa-9fd84ca064b0"), new DateTime(2025, 7, 31, 6, 25, 2, 170, DateTimeKind.Utc).AddTicks(3974), "", "financehr@timesheettracker.com", "Finance", true, "HR", "$2a$11$tdYdg0pFBDcxrjuwPerViOKLyNnGC6MTGLAyGCXTN7gqebxCBTP1q", "FinanceHR", new DateTime(2025, 7, 31, 6, 25, 2, 170, DateTimeKind.Utc).AddTicks(3979) }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("77b82e92-ffa1-45ce-a820-9206711f27f5"), 10000m, "Internal", new DateTime(2025, 7, 31, 6, 25, 2, 170, DateTimeKind.Utc).AddTicks(4749), new Guid("6299a0b7-e6cd-453d-bb48-246439c40724"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 31, 6, 25, 2, 170, DateTimeKind.Utc).AddTicks(4750) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("77b82e92-ffa1-45ce-a820-9206711f27f5"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4171f0df-44bb-44af-835e-dc6916616b39"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8a59ff26-bd9a-4c83-bfc2-92f8dd10a311"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dbf0c851-44e1-409b-87aa-9fd84ca064b0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6299a0b7-e6cd-453d-bb48-246439c40724"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("a9b886cc-2591-49a4-8216-dd1025ded77c"), new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(3976), "", "admin@timesheettracker.com", "System", true, "Administrator", "$2a$11$TrfNUJF5tT6nav2uOxttFO46t7gghP992GuQvg4FLzgtvw76BwL96", "Admin", new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(3981) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "ClientName", "CreatedAt", "CreatedBy", "Description", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[] { new Guid("724451a5-3af1-4fc6-87de-5b262f60b0a8"), 10000m, "Internal", new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(4741), new Guid("a9b886cc-2591-49a4-8216-dd1025ded77c"), "This is a sample project for demonstration purposes", new DateTime(2025, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), true, "Sample Project", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 30, 8, 26, 29, 973, DateTimeKind.Utc).AddTicks(4742) });
        }
    }
}
