using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingTrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts",
                column: "UserListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserListId",
                table: "UserProducts",
                column: "UserListId",
                unique: true);
        }
    }
}
