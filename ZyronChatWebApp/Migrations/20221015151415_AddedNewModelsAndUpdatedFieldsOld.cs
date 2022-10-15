using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZyronChat.Data.Migrations
{
    public partial class AddedNewModelsAndUpdatedFieldsOld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    IdUserSender = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUserReceiver = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserSenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => new { x.IdUserSender, x.IdUserReceiver });
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_UserReceiverId",
                        column: x => x.UserReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_UserSenderId",
                        column: x => x.UserSenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSended = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSended = table.Column<TimeSpan>(type: "time", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatMessagesIdUserReceiver = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChatMessagesIdUserSender = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ChatMessages_ChatMessagesIdUserSender_ChatMessagesIdUserReceiver",
                        columns: x => new { x.ChatMessagesIdUserSender, x.ChatMessagesIdUserReceiver },
                        principalTable: "ChatMessages",
                        principalColumns: new[] { "IdUserSender", "IdUserReceiver" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserReceiverId",
                table: "ChatMessages",
                column: "UserReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserSenderId",
                table: "ChatMessages",
                column: "UserSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatMessagesIdUserSender_ChatMessagesIdUserReceiver",
                table: "Messages",
                columns: new[] { "ChatMessagesIdUserSender", "ChatMessagesIdUserReceiver" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ChatMessages");
        }
    }
}
