using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 2, "Description 2", "Product 2", 20.00m, 200 },
                    { 3, "Description 3", "Product 3", 30.00m, 300 },
                    { 4, "Description 4", "Product 4", 40.00m, 400 },
                    { 5, "Description 5", "Product 5", 50.00m, 500 },
                    { 6, "Description 6", "Product 6", 60.00m, 600 },
                    { 7, "Description 7", "Product 7", 70.00m, 700 },
                    { 8, "Description 8", "Product 8", 80.00m, 800 },
                    { 9, "Description 9", "Product 9", 90.00m, 900 },
                    { 10, "Description 10", "Product 10", 100.00m, 1000 },
                    { 11, "Description 11", "Product 11", 110.00m, 1100 },
                    { 12, "Description 12", "Product 12", 120.00m, 1200 },
                    { 13, "Description 13", "Product 13", 130.00m, 1300 },
                    { 14, "Description 14", "Product 14", 140.00m, 1400 },
                    { 15, "Description 15", "Product 15", 150.00m, 1500 },
                    { 16, "Description 16", "Product 16", 160.00m, 1600 },
                    { 17, "Description 17", "Product 17", 170.00m, 1700 },
                    { 18, "Description 18", "Product 18", 180.00m, 1800 },
                    { 19, "Description 19", "Product 19", 190.00m, 1900 },
                    { 20, "Description 20", "Product 20", 200.00m, 2000 },
                    { 21, "Description 21", "Product 21", 210.00m, 2100 },
                    { 22, "Description 22", "Product 22", 220.00m, 2200 },
                    { 23, "Description 23", "Product 23", 230.00m, 2300 },
                    { 24, "Description 24", "Product 24", 240.00m, 2400 },
                    { 25, "Description 25", "Product 25", 250.00m, 2500 },
                    { 26, "Description 26", "Product 26", 260.00m, 2600 },
                    { 27, "Description 27", "Product 27", 270.00m, 2700 },
                    { 28, "Description 28", "Product 28", 280.00m, 2800 },
                    { 29, "Description 29", "Product 29", 290.00m, 2900 },
                    { 30, "Description 30", "Product 30", 300.00m, 3000 },
                    { 31, "Description 31", "Product 31", 310.00m, 3100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);
        }
    }
}
