using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBuy.Data.Migrations
{
    public partial class ProductDateListed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateListed",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateListed",
                table: "Products");
        }
    }
}
