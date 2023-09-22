using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MinerContainerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContainerName",
                table: "Miners",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerName",
                table: "Miners");
        }
    }
}
