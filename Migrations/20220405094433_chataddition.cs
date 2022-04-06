using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalIntercomProject.Migrations
{
    public partial class chataddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatIdentity",
                table: "UsersTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Chatthreaduserstable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatthreaduserstable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chatthreaduserstable_UsersTable_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "UsersTable",
                keyColumn: "Id",
                keyValue: 1,
                column: "ChatIdentity",
                value: "8:acs:55cdd872-42d3-4c8c-a242-f2191f9c8b94_00000010-838f-0201-f40f-343a0d0033b9");

            migrationBuilder.CreateIndex(
                name: "IX_Chatthreaduserstable_UserId",
                table: "Chatthreaduserstable",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chatthreaduserstable");

            migrationBuilder.DropColumn(
                name: "ChatIdentity",
                table: "UsersTable");
        }
    }
}
