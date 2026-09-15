using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceFlow.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "EstimatedMinutes", "IsActive", "Name", "StartingPrice" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Mowing, edging, and general yard cleanup.", 90, true, "Lawn Care", 75.00m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Exterior cleaning for driveways, patios, and siding.", 120, true, "Pressure Washing", 150.00m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Removal of leaves and debris from gutters and downspouts.", 90, true, "Gutter Cleaning", 125.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
