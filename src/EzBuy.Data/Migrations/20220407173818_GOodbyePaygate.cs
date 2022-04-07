﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBuy.Data.Migrations
{
    public partial class GOodbyePaygate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TRANSACTION_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AMOUNT = table.Column<int>(type: "int", nullable: false),
                    CUSTOMER_EMAIL_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PAY_REQUEST_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REFERENCE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RESULT_CODE = table.Column<int>(type: "int", nullable: false),
                    RESULT_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANSACTION_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TRANSACTION_ID);
                });
        }
    }
}
