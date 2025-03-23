using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineShop.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addSeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Manufacturer", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 3, "Electronics", "Portable tablet for everyday use", "Apple", "Tablet", 600.0, 30 },
                    { 4, "Wearables", "Wearable smart device", "Fitbit", "Smartwatch", 250.0, 40 },
                    { 5, "Audio", "Noise-cancelling headphones", "Bose", "Headphones", 300.0, 15 },
                    { 6, "Photography", "Digital SLR camera", "Canon", "Camera", 1200.0, 10 },
                    { 7, "Office", "Wireless printer", "HP", "Printer", 200.0, 25 },
                    { 8, "Computers", "4K Ultra HD monitor", "LG", "Monitor", 400.0, 18 },
                    { 9, "Computers", "Mechanical keyboard", "Logitech", "Keyboard", 100.0, 35 },
                    { 10, "Computers", "Wireless mouse", "Microsoft", "Mouse", 50.0, 45 },
                    { 11, "Gaming", "Next-gen gaming console", "Sony", "Gaming Console", 500.0, 22 },
                    { 12, "Gaming", "Immersive virtual reality experience", "Oculus", "VR Headset", 350.0, 12 },
                    { 13, "Networking", "High-speed wireless router", "Netgear", "Router", 150.0, 28 },
                    { 14, "Computers", "Portable storage device", "Seagate", "External Hard Drive", 100.0, 40 },
                    { 15, "Electronics", "4K smart television", "Samsung", "Smart TV", 900.0, 8 },
                    { 16, "Audio", "Portable wireless speaker", "JBL", "Bluetooth Speaker", 120.0, 30 },
                    { 17, "Wearables", "Wearable fitness tracker", "Garmin", "Fitness Tracker", 80.0, 50 },
                    { 18, "Photography", "Quadcopter drone with camera", "DJI", "Drone", 750.0, 5 },
                    { 19, "Electronics", "Digital book reader", "Amazon", "E-Reader", 130.0, 20 },
                    { 20, "Home", "Wi-Fi enabled smart bulb", "Philips", "Smart Light", 30.0, 100 },
                    { 21, "Accessories", "Fast wireless charging pad", "Anker", "Wireless Charger", 40.0, 60 },
                    { 22, "Photography", "Rugged action camera", "GoPro", "Action Camera", 250.0, 14 },
                    { 23, "Accessories", "Ergonomic laptop stand", "Rain Design", "Laptop Stand", 70.0, 30 },
                    { 24, "Audio", "True wireless stereo earbuds", "Apple", "Wireless Earbuds", 200.0, 25 },
                    { 25, "Accessories", "High-capacity power bank", "RAVPower", "Portable Charger", 60.0, 35 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);
        }
    }
}
