using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace autoCadApiDevelopment.Migrations
{
    /// <inheritdoc />
    public partial class filePathforUploadfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "UploadFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "UploadFile");
        }
    }
}
