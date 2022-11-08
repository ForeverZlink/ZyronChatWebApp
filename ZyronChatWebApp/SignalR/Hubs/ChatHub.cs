using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Controllers;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public ChatMessagesLogic ChatMessageLogic {get;set;}
        
        public ChatHub(UserContext dbcontext)
        {

            this.ChatMessageLogic = new ChatMessagesLogic(dbcontext);

            
        }
        public async  Task SendMessagePrivate(string userToSendUsername, string message)
        {
            string userWhoSend = this.Context.User.Identity.Name;

            this.ChatMessageLogic.SaveMessagesChatBetweenTwoUsers(userWhoSend, userToSendUsername, message);
            var idUserToSend = this.ChatMessageLogic.SearchUserIdWithUsername(userToSendUsername); 
            
            //Send a private message for a other user. 1x1 chat. To work, its necessary pass the Id field of User, username not working,
            //just the id
            await this.Clients.User(idUserToSend).SendAsync("ReceiveMessagePrivate", message);
           
        }
        public async Task SendMessageToAll(string user, string message)
        {
            
            await this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
