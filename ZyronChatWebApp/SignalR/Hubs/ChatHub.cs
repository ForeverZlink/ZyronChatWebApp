using Microsoft.AspNetCore.SignalR;

namespace ZyronChatWebApp.SignalR.Hubs
{
    public class ChatHub:Hub
    {
        public  Task SendMessagePrivate(string user, string message)
        {
            //Send a private message for a other user. 1x1 chat
            
            return  this.Clients.User(user).SendAsync("ReceiveMessagePrivate", message);
           
        }
        public Task SendMessageToAll(string user, string message)
        {
            
            return this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
