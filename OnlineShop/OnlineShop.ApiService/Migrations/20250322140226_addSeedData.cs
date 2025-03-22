using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineShop.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BillingAddresses",
                columns: new[] { "Id", "City", "Country", "HouseNum", "State", "Street", "ZipCode" },
                values: new object[] { 2, "Springfield", "USA", "2B", "IL", "456 Elm St", "62704" });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "TotalPrice" },
                values: new object[] { 1, 1500.0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Manufacturer", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, "Electronics", "A powerful laptop", "Dell", "Laptop", 1500.0, 20 },
                    { 2, "Electronics", "Latest model smartphone", "Samsung", "Smartphone", 800.0, 50 }
                });

            migrationBuilder.InsertData(
                table: "ShippingAddresses",
                columns: new[] { "Id", "City", "Country", "HouseNum", "State", "Street", "ZipCode" },
                values: new object[] { 1, "Springfield", "USA", "1A", "IL", "123 Main St", "62704" });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CartId", "Price", "ProductId", "Quantity" },
                values: new object[] { 1, 1, 1500.0, 1, 1 });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ApplicationUserId", "BillingAddressId", "CartId", "FirstName", "LastName", "PhoneNumber", "ShippingAddressId" },
                values: new object[] { 1, null, 2, 1, "John", "Doe", "555-1234", 1 });

            migrationBuilder.InsertData(
                table: "SalesOrders",
                columns: new[] { "Id", "BillingAddressId", "CartId", "CustomerId", "ShippingAddressId" },
                values: new object[] { 1, 2, 1, 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SalesOrders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BillingAddresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShippingAddresses",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
