using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SingleFSForFarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Farms_FarmId",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_FarmId",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "FarmId",
                table: "FlightSheets");

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveFlightSheetId",
                table: "Farms",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ActiveFlightSheetId",
                table: "Farms",
                column: "ActiveFlightSheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_FlightSheets_ActiveFlightSheetId",
                table: "Farms",
                column: "ActiveFlightSheetId",
                principalTable: "FlightSheets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_FlightSheets_ActiveFlightSheetId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ActiveFlightSheetId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ActiveFlightSheetId",
                table: "Farms");

            migrationBuilder.AddColumn<Guid>(
                name: "FarmId",
                table: "FlightSheets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_FarmId",
                table: "FlightSheets",
                column: "FarmId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Farms_FarmId",
                table: "FlightSheets",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id");
        }
    }
}
