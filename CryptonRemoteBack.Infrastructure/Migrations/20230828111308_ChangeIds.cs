using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_FlightSheets_ActiveFlightSheetId",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Miners_MinerId",
                table: "FlightSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Pools_PoolId",
                table: "FlightSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Wallets_WalletId",
                table: "FlightSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_PoolAddresses_Pools_PoolId",
                table: "PoolAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Currencies_CurrencyId",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_CurrencyId",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pools",
                table: "Pools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PoolAddresses",
                table: "PoolAddresses");

            migrationBuilder.DropIndex(
                name: "IX_PoolAddresses_PoolId",
                table: "PoolAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Miners",
                table: "Miners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightSheets",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_MinerId",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_PoolId",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_WalletId",
                table: "FlightSheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Farms",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ActiveFlightSheetId",
                table: "Farms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Pools");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PoolAddresses");

            migrationBuilder.DropColumn(
                name: "PoolId",
                table: "PoolAddresses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Miners");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "MinerId",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "PoolId",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ActiveFlightSheetId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Currencies");

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "Wallets",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId2",
                table: "Wallets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "Pools",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "PoolAddresses",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "PoolId2",
                table: "PoolAddresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "Miners",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "FlightSheets",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "MinerId2",
                table: "FlightSheets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PoolId2",
                table: "FlightSheets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WalletId2",
                table: "FlightSheets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "Farms",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlightSheetId2",
                table: "Farms",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "Currencies",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pools",
                table: "Pools",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PoolAddresses",
                table: "PoolAddresses",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Miners",
                table: "Miners",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightSheets",
                table: "FlightSheets",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Farms",
                table: "Farms",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id2");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CurrencyId2",
                table: "Wallets",
                column: "CurrencyId2");

            migrationBuilder.CreateIndex(
                name: "IX_PoolAddresses_PoolId2",
                table: "PoolAddresses",
                column: "PoolId2");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_MinerId2",
                table: "FlightSheets",
                column: "MinerId2");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_PoolId2",
                table: "FlightSheets",
                column: "PoolId2");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_WalletId2",
                table: "FlightSheets",
                column: "WalletId2");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ActiveFlightSheetId2",
                table: "Farms",
                column: "ActiveFlightSheetId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_FlightSheets_ActiveFlightSheetId2",
                table: "Farms",
                column: "ActiveFlightSheetId2",
                principalTable: "FlightSheets",
                principalColumn: "Id2");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Miners_MinerId2",
                table: "FlightSheets",
                column: "MinerId2",
                principalTable: "Miners",
                principalColumn: "Id2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Pools_PoolId2",
                table: "FlightSheets",
                column: "PoolId2",
                principalTable: "Pools",
                principalColumn: "Id2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Wallets_WalletId2",
                table: "FlightSheets",
                column: "WalletId2",
                principalTable: "Wallets",
                principalColumn: "Id2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PoolAddresses_Pools_PoolId2",
                table: "PoolAddresses",
                column: "PoolId2",
                principalTable: "Pools",
                principalColumn: "Id2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Currencies_CurrencyId2",
                table: "Wallets",
                column: "CurrencyId2",
                principalTable: "Currencies",
                principalColumn: "Id2",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_FlightSheets_ActiveFlightSheetId2",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Miners_MinerId2",
                table: "FlightSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Pools_PoolId2",
                table: "FlightSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSheets_Wallets_WalletId2",
                table: "FlightSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_PoolAddresses_Pools_PoolId2",
                table: "PoolAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Currencies_CurrencyId2",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_CurrencyId2",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pools",
                table: "Pools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PoolAddresses",
                table: "PoolAddresses");

            migrationBuilder.DropIndex(
                name: "IX_PoolAddresses_PoolId2",
                table: "PoolAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Miners",
                table: "Miners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightSheets",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_MinerId2",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_PoolId2",
                table: "FlightSheets");

            migrationBuilder.DropIndex(
                name: "IX_FlightSheets_WalletId2",
                table: "FlightSheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Farms",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ActiveFlightSheetId2",
                table: "Farms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "CurrencyId2",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Pools");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "PoolAddresses");

            migrationBuilder.DropColumn(
                name: "PoolId2",
                table: "PoolAddresses");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Miners");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "MinerId2",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "PoolId2",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "WalletId2",
                table: "FlightSheets");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ActiveFlightSheetId2",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Currencies");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Wallets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "Wallets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Pools",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PoolAddresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PoolId",
                table: "PoolAddresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Miners",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FlightSheets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MinerId",
                table: "FlightSheets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PoolId",
                table: "FlightSheets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "FlightSheets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Farms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveFlightSheetId",
                table: "Farms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Currencies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pools",
                table: "Pools",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PoolAddresses",
                table: "PoolAddresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Miners",
                table: "Miners",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightSheets",
                table: "FlightSheets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Farms",
                table: "Farms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CurrencyId",
                table: "Wallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolAddresses_PoolId",
                table: "PoolAddresses",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_MinerId",
                table: "FlightSheets",
                column: "MinerId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_PoolId",
                table: "FlightSheets",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_WalletId",
                table: "FlightSheets",
                column: "WalletId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Miners_MinerId",
                table: "FlightSheets",
                column: "MinerId",
                principalTable: "Miners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Pools_PoolId",
                table: "FlightSheets",
                column: "PoolId",
                principalTable: "Pools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSheets_Wallets_WalletId",
                table: "FlightSheets",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PoolAddresses_Pools_PoolId",
                table: "PoolAddresses",
                column: "PoolId",
                principalTable: "Pools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Currencies_CurrencyId",
                table: "Wallets",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
