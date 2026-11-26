using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab6_6minimalapi_vinnik.Migrations
{
    /// <inheritdoc />
    public partial class SeedAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountNumber", "AccountType", "Balance", "CreatedAt", "IsActive", "Owner" },
                values: new object[,]
                {
                    { 1, "BY001", "Сберегательный", 2500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Иван Иванов" },
                    { 2, "BY002", "Кредитный", 5000m, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), true, "Анна Петрова" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
