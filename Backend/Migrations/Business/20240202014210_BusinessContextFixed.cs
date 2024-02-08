using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend1.Migrations.Business
{
    /// <inheritdoc />
    public partial class BusinessContextFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Businesses");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Businesses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Businesses");

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "Businesses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Businesses",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "Businesses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
