using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingTrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts");

            migrationBuilder.AddColumn<string>(
                name: "ProductImg",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts",
                column: "UserListId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "ProductImg",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts",
                column: "UserListId");
        }
    }
}
