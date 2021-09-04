using Microsoft.EntityFrameworkCore.Migrations;

namespace CardFile.DAL.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTextFiles_AspNetUsers_UserId",
                table: "CardTextFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardTextFiles",
                table: "CardTextFiles");

            migrationBuilder.RenameTable(
                name: "CardTextFiles",
                newName: "CardFiles");

            migrationBuilder.RenameIndex(
                name: "IX_CardTextFiles_UserId",
                table: "CardFiles",
                newName: "IX_CardFiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardFiles",
                table: "CardFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardFiles_AspNetUsers_UserId",
                table: "CardFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardFiles_AspNetUsers_UserId",
                table: "CardFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardFiles",
                table: "CardFiles");

            migrationBuilder.RenameTable(
                name: "CardFiles",
                newName: "CardTextFiles");

            migrationBuilder.RenameIndex(
                name: "IX_CardFiles_UserId",
                table: "CardTextFiles",
                newName: "IX_CardTextFiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardTextFiles",
                table: "CardTextFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardTextFiles_AspNetUsers_UserId",
                table: "CardTextFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
