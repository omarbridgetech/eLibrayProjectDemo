using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLibrayProjectDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousPriceToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousPrice",
                table: "Books",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousPrice",
                table: "Books");
        }
    }
}
