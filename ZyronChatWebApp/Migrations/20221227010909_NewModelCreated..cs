using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZyronChatWebApp.Migrations
{
    public partial class NewModelCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationsId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUserCreatorOfNotification = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_UserPublic_IdUserCreatorOfNotification",
                        column: x => x.IdUserCreatorOfNotification,
                        principalTable: "UserPublic",
                        principalColumn: "IdPublic",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_NotificationsId",
                table: "Messages",
                column: "NotificationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IdUserCreatorOfNotification",
                table: "Notifications",
                column: "IdUserCreatorOfNotification",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Notifications_NotificationsId",
                table: "Messages",
                column: "NotificationsId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Notifications_NotificationsId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Messages_NotificationsId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "NotificationsId",
                table: "Messages");
        }
    }
}
