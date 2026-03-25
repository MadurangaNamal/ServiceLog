using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLog.Data.Migrations
{
    /// <inheritdoc />
    public partial class typo_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChasisNumber",
                table: "Vehicles",
                newName: "ChassisNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChassisNumber",
                table: "Vehicles",
                newName: "ChasisNumber");
        }
    }
}
