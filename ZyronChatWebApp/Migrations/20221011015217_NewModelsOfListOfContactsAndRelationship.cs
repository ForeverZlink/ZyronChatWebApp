using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZyronChat.Data.Migrations
{
    public partial class NewModelsOfListOfContactsAndRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserScheduleListOfContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScheduleListOfContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserScheduleListOfContacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsernameOfIdentification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserScheduleListOfContacts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInformations_UserScheduleListOfContacts_IdUserScheduleListOfContacts",
                        column: x => x.IdUserScheduleListOfContacts,
                        principalTable: "UserScheduleListOfContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformations_IdUserScheduleListOfContacts",
                table: "ContactInformations",
                column: "IdUserScheduleListOfContacts");

            migrationBuilder.CreateIndex(
                name: "IX_UserScheduleListOfContacts_UserId",
                table: "UserScheduleListOfContacts",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInformations");

            migrationBuilder.DropTable(
                name: "UserScheduleListOfContacts");
        }
    }
}
