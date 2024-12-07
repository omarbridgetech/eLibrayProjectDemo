using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLibrayProjectDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfFilePathToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EpubFilePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "F2bFilePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobiFilePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PdfFilePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpubFilePath",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "F2bFilePath",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MobiFilePath",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PdfFilePath",
                table: "Books");
        }
    }
}
