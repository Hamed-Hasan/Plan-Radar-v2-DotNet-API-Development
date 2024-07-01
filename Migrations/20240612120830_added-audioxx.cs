using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace autoCadApiDevelopment.Migrations
{
    /// <inheritdoc />
    public partial class addedaudioxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Pins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Pins");
        }
    }
}
