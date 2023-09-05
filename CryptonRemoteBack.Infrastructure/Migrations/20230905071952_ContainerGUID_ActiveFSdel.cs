using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContainerGUID_ActiveFSdel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FlightSheets");

            migrationBuilder.AddColumn<string>(
                name: "ContainerGUID",
                table: "Farms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerGUID",
                table: "Farms");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FlightSheets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
