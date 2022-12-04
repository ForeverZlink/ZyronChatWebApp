using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Controllers;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using ZyronChatWebApp.Models;
using System.Collections.Immutable;

namespace ZyronChatWebApp.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public ChatMessagesLogic ChatMessageLogic {get;set;}
        public UserContext dbcontext { get; set; }
        public ChatHub(UserContext dbcontext)
        {
            this.dbcontext = dbcontext;
            this.ChatMessageLogic = new ChatMessagesLogic(dbcontext);
            
            
        }
        public async  Task SendMessagePrivate(string IdPublicUserToSend, string message)
        {
            string IdUserCaller = this.Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            
            var UserPublic = this.dbcontext.UserPublic.FirstOrDefault(x => x.IdPrivate==IdUserCaller);
            var UserToSendPublic = this.dbcontext.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserToSend);

            this.ChatMessageLogic.SaveMessagesChatBetweenTwoUsers(UserPublic.IdPublic, UserToSendPublic.IdPublic, message);
            
            //Send a private message for a other user. 1x1 chat. To work, its necessary pass the Id field of User, username not working,
            //just the id
            await this.Clients.User(UserToSendPublic.IdPrivate).SendAsync("ReceiveMessagePrivate", message);
           
        }
        public async Task SendMessageToAll(string user, string message)
        {
            
            await this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
