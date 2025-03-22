using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineShop.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BillingAddresses",
                columns: new[] { "Id", "City", "Country", "HouseNum", "State", "Street", "ZipCode" },
                values: new object[,]
                {
                    { 3, "Wolfsberg", "Österreich", "15", "Kärnten", "Lausing", "9411" },
                    { 5, "St. Michale", "Österreich", "5", "Kärnten", "Lausing", "9411" },
                    { 7, "Klagenfurt", "Kärnten", "1", "Kärnten", "Arnulfplatz", "9010" }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "TotalPrice" },
                values: new object[,]
                {
                    { 3, 0.0 },
                    { 4, 0.0 },
                    { 5, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ApplicationUserId", "BillingAddressId", "CartId", "FirstName", "LastName", "PhoneNumber", "ShippingAddressId" },
                values: new object[] { 4, null, 2, 1, "Andre", "Ludwig", "555-1234", 1 });

            migrationBuilder.UpdateData(
                table: "SalesOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "CustomerId",
                value: 4);

            migrationBuilder.InsertData(
                table: "ShippingAddresses",
                columns: new[] { "Id", "City", "Country", "HouseNum", "State", "Street", "ZipCode" },
                values: new object[,]
                {
                    { 4, "Wolfsberg", "Österreich", "15", "Kärnten", "Lausing", "9411" },
                    { 6, "St. Michale", "Österreich", "5", "Kärnten", "Lausing", "9411" },
                    { 8, "Klagenfurt", "Kärnten", "1", "Kärnten", "Arnulfplatz", "9010" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BillingAddresses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BillingAddresses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BillingAddresses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ShippingAddresses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ShippingAddresses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ShippingAddresses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ApplicationUserId", "BillingAddressId", "CartId", "FirstName", "LastName", "PhoneNumber", "ShippingAddressId" },
                values: new object[] { 1, null, 2, 1, "John", "Doe", "555-1234", 1 });

            migrationBuilder.UpdateData(
                table: "SalesOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "CustomerId",
                value: 1);
        }
    }
}
