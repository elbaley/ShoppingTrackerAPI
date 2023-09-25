using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingTrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserList_Users_UserId",
                table: "UserList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_UserList_UserListId",
                table: "UserProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserList",
                table: "UserList");

            migrationBuilder.RenameTable(
                name: "UserList",
                newName: "UserLists");

            migrationBuilder.RenameIndex(
                name: "IX_UserList_UserId",
                table: "UserLists",
                newName: "IX_UserLists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserList_Name",
                table: "UserLists",
                newName: "IX_UserLists_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLists",
                table: "UserLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLists_Users_UserId",
                table: "UserLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_UserLists_UserListId",
                table: "UserProduct",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLists_Users_UserId",
                table: "UserLists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_UserLists_UserListId",
                table: "UserProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLists",
                table: "UserLists");

            migrationBuilder.RenameTable(
                name: "UserLists",
                newName: "UserList");

            migrationBuilder.RenameIndex(
                name: "IX_UserLists_UserId",
                table: "UserList",
                newName: "IX_UserList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLists_Name",
                table: "UserList",
                newName: "IX_UserList_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserList",
                table: "UserList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserList_Users_UserId",
                table: "UserList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_UserList_UserListId",
                table: "UserProduct",
                column: "UserListId",
                principalTable: "UserList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
