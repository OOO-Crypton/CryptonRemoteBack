using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptonRemoteBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCoreFreqNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "СoreFrequency",
                table: "OverclockingParams",
                newName: "CoreFrequency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoreFrequency",
                table: "OverclockingParams",
                newName: "СoreFrequency");
        }
    }
}
