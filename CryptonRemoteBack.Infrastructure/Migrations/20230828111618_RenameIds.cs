using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CurrencyId2",
                table: "Wallets",
                newName: "CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Wallets",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_CurrencyId2",
                table: "Wallets",
                newName: "IX_Wallets_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Pools",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PoolId2",
                table: "PoolAddresses",
                newName: "PoolId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "PoolAddresses",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_PoolAddresses_PoolId2",
                table: "PoolAddresses",
                newName: "IX_PoolAddresses_PoolId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Miners",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "WalletId2",
                table: "FlightSheets",
                newName: "WalletId");

            migrationBuilder.RenameColumn(
                name: "PoolId2",
                table: "FlightSheets",
                newName: "PoolId");

            migrationBuilder.RenameColumn(
                name: "MinerId2",
                table: "FlightSheets",
                newName: "MinerId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "FlightSheets",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSheets_WalletId2",
                table: "FlightSheets",
                newName: "IX_FlightSheets_WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSheets_PoolId2",
                table: "FlightSheets",
                newName: "IX_FlightSheets_PoolId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSheets_MinerId2",
                table: "FlightSheets",
                newName: "IX_FlightSheets_MinerId");

            migrationBuilder.RenameColumn(
                name: "ActiveFlightSheetId2",
                table: "Farms",
                newName: "ActiveFlightSheetId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Farms",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Farms_ActiveFlightSheetId2",
                table: "Farms",
                newName: "IX_Farms_ActiveFlightSheetId");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Currencies",
                newName: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Wallets",
                newName: "CurrencyId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Wallets",
                newName: "Id2");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_CurrencyId",
                table: "Wallets",
                newName: "IX_Wallets_CurrencyId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Pools",
                newName: "Id2");

            migrationBuilder.RenameColumn(
                name: "PoolId",
                table: "PoolAddresses",
                newName: "PoolId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PoolAddresses",
                newName: "Id2");

            migrationBuilder.RenameIndex(
                name: "IX_PoolAddresses_PoolId",
                table: "PoolAddresses",
                newName: "IX_PoolAddresses_PoolId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Miners",
                newName: "Id2");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "FlightSheets",
                newName: "WalletId2");

            migrationBuilder.RenameColumn(
                name: "PoolId",
                table: "FlightSheets",
                newName: "PoolId2");

            migrationBuilder.RenameColumn(
                name: "MinerId",
                table: "FlightSheets",
                newName: "MinerId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FlightSheets",
                newName: "Id2");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSheets_WalletId",
                table: "FlightSheets",
                newName: "IX_FlightSheets_WalletId2");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSheets_PoolId",
                table: "FlightSheets",
                newName: "IX_FlightSheets_PoolId2");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSheets_MinerId",
                table: "FlightSheets",
                newName: "IX_FlightSheets_MinerId2");

            migrationBuilder.RenameColumn(
                name: "ActiveFlightSheetId",
                table: "Farms",
                newName: "ActiveFlightSheetId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Farms",
                newName: "Id2");

            migrationBuilder.RenameIndex(
                name: "IX_Farms_ActiveFlightSheetId",
                table: "Farms",
                newName: "IX_Farms_ActiveFlightSheetId2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Currencies",
                newName: "Id2");

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
    }
}
