using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerOrdersService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<decimal>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Email", "Name" },
                values: new object[] { 1, "john.doe@a.com", "John Doe" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Email", "Name" },
                values: new object[] { 2, "isaac.newton@b.com", "Isaac Newton" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Email", "Name" },
                values: new object[] { 3, "lady.ada@c.com", "Lady Ada" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CreatedDate", "CustomerId", "Price" },
                values: new object[] { 1, new DateTime(2019, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 500.21m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CreatedDate", "CustomerId", "Price" },
                values: new object[] { 2, new DateTime(2019, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1000.32m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CreatedDate", "CustomerId", "Price" },
                values: new object[] { 3, new DateTime(2019, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 800.65m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CreatedDate", "CustomerId", "Price" },
                values: new object[] { 4, new DateTime(2019, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 100.43m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CreatedDate", "CustomerId", "Price" },
                values: new object[] { 5, new DateTime(2019, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 300.56m });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
