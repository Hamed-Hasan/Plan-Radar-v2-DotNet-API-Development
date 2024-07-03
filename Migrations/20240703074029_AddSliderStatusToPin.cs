using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace autoCadApiDevelopment.Migrations
{
    /// <inheritdoc />
    public partial class AddSliderStatusToPin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SliderStatus",
                table: "Pins",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SliderStatus",
                table: "Pins");
        }
    }
}
