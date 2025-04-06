using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class changeOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_BillingAddresses_BillingAddressId",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Carts_CartId",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_ShippingAddresses_ShippingAddressId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_BillingAddressId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_CartId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_ShippingAddressId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "SalesOrders");

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCountry",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingHouseNum",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingName",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingState",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingStreet",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingZipCode",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipCity",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipCountry",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipHouseNum",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipName",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipState",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipStreet",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipZipCode",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippedDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_SalesOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[] { 1, 1, 1, 1, 40.200000000000003 });

            migrationBuilder.UpdateData(
                table: "SalesOrders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BillingCity", "BillingCountry", "BillingHouseNum", "BillingName", "BillingState", "BillingStreet", "BillingZipCode", "OrderDate", "ShipCity", "ShipCountry", "ShipHouseNum", "ShipName", "ShipState", "ShipStreet", "ShipZipCode", "ShippedDate" },
                values: new object[] { "Klagenfurt", "Austria", "1", "Martha Liu", "Kärnten", "Lausing", "9020", new DateTime(2025, 3, 25, 19, 26, 56, 480, DateTimeKind.Local).AddTicks(990), "Klagenfurt", "Austria", "1", "Andre Ludwig", "Kärnten", "Lausing", "9020", new DateTime(2025, 3, 25, 19, 26, 56, 480, DateTimeKind.Local).AddTicks(1036) });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingCountry",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingHouseNum",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingName",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingState",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingStreet",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingZipCode",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipCity",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipCountry",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipHouseNum",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipName",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipState",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipStreet",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShipZipCode",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShippedDate",
                table: "SalesOrders");

            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SalesOrders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BillingAddressId", "CartId", "OrderDate", "ShippingAddressId" },
                values: new object[] { 2, 1, new DateTime(2025, 3, 25, 12, 11, 17, 894, DateTimeKind.Local).AddTicks(3858), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_BillingAddressId",
                table: "SalesOrders",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CartId",
                table: "SalesOrders",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_ShippingAddressId",
                table: "SalesOrders",
                column: "ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_BillingAddresses_BillingAddressId",
                table: "SalesOrders",
                column: "BillingAddressId",
                principalTable: "BillingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Carts_CartId",
                table: "SalesOrders",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_ShippingAddresses_ShippingAddressId",
                table: "SalesOrders",
                column: "ShippingAddressId",
                principalTable: "ShippingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
