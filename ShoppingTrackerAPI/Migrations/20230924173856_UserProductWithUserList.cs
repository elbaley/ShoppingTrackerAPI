using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingTrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserProductWithUserList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_UserList_UserListId",
                table: "UserProduct");

            migrationBuilder.AlterColumn<int>(
                name: "UserListId",
                table: "UserProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserList_Name",
                table: "UserList",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_UserList_UserListId",
                table: "UserProduct",
                column: "UserListId",
                principalTable: "UserList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_UserList_UserListId",
                table: "UserProduct");

            migrationBuilder.DropIndex(
                name: "IX_UserList_Name",
                table: "UserList");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "UserListId",
                table: "UserProduct",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_UserList_UserListId",
                table: "UserProduct",
                column: "UserListId",
                principalTable: "UserList",
                principalColumn: "Id");
        }
    }
}
