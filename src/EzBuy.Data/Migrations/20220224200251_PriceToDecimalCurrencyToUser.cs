using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBuy.Data.Migrations
{
    public partial class PriceToDecimalCurrencyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EzBucks",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<decimal>(
                name: "EzBucks",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EzBucks",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "EzBucks",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
