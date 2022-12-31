using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZyronChatWebApp.Migrations
{
    public partial class NewFieldOfMessagesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Notified",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notified",
                table: "Messages");
        }
    }
}
