using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PoolToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Pools_PoolId",
                table: "FlightSheets");

            migrationBuilder.DropTable(
                name: "PoolAddresses");

            migrationBuilder.DropTable(
                name: "Pools");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_PoolId",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "PoolId",
                table: "FlightSheets");

            migrationBuilder.AddColumn<string>(
                name: "PoolAddress",
                table: "FlightSheets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PoolAddress",
                table: "FlightSheets");

            migrationBuilder.AddColumn<int>(
                name: "PoolId",
                table: "FlightSheets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoolAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PoolId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoolAddresses_Pools_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_PoolId",
                table: "FlightSheets",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolAddresses_PoolId",
                table: "PoolAddresses",
                column: "PoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Pools_PoolId",
                table: "FlightSheets",
                column: "PoolId",
                principalTable: "Pools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
