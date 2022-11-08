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
            
            return  this.Clients.User(user).SendAsync("ReceiveMessagePrivate", message);
           
        }
        public Task SendMessageToAll(string user, string message)
        {
            
            return this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
