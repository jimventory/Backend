using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend1.Migrations
{
    /// <inheritdoc />
    public partial class _30salescounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Sales",
                table: "Items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sales",
                table: "Items");
        }
    }
}
