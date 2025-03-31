using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "Stock" },
                values: new object[] { 1, "Description 1", "Product 1", 10.00m, 100 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.DeleteData(
                    table: "Products",
                    keyColumn: "Id",
                keyValue: 1);
        }
    }
}
