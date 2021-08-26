using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardFile.DAL.Migrations.CardFileDb
{
    public partial class updateCardFileEntitie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "CardTextFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "CardTextFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "CardTextFiles");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "CardTextFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
