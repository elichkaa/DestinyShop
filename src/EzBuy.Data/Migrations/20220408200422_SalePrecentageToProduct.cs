using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBuy.Data.Migrations
{
    public partial class SalePrecentageToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleOffPrecentage",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleOffPrecentage",
                table: "Products");
        }
    }
}
