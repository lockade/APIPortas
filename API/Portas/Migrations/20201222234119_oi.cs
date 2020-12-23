using Microsoft.EntityFrameworkCore.Migrations;

namespace Portas.Migrations
{
    public partial class oi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    senhaEncry = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBUser", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBUser_email",
                table: "TBUser",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBUser");
        }
    }
}
