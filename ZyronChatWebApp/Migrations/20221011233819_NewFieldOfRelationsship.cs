using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZyronChat.Data.Migrations
{
    public partial class NewFieldOfRelationsship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserScheduleListOfContacts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserScheduleListOfContacts_UserId",
                table: "UserScheduleListOfContacts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserScheduleListOfContacts_AspNetUsers_UserId",
                table: "UserScheduleListOfContacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserScheduleListOfContacts_AspNetUsers_UserId",
                table: "UserScheduleListOfContacts");

            migrationBuilder.DropIndex(
                name: "IX_UserScheduleListOfContacts_UserId",
                table: "UserScheduleListOfContacts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserScheduleListOfContacts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
