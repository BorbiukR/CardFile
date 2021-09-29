using Microsoft.EntityFrameworkCore.Migrations;

namespace CardFile.DAL.Migrations
{
    public partial class SepareteTableCardFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileInfoEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    CardFileEntitieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfoEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileInfoEntities_CardFiles_CardFileEntitieId",
                        column: x => x.CardFileEntitieId,
                        principalTable: "CardFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileInfoEntities_CardFileEntitieId",
                table: "FileInfoEntities",
                column: "CardFileEntitieId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileInfoEntities");
        }
    }
}
