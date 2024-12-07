using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLibrayProjectDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountStartDateToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DiscountStartDate",
                table: "Books",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountStartDate",
                table: "Books");
        }
    }
}
