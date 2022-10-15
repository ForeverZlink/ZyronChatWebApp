using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZyronChatWebApp.Models
{
    public class ChatMessages 
    {
        [Key]
        public int Id { get; set; }

        public string IdUserSender { get; set; }
        public UserModelCustom UserSender { get; set; }

        public string IdUserReceiver { get; set; }
        public UserModelCustom UserReceiver { get; set; }

        public ICollection<Messages> MessagesList { get; set; }

            
    }
}
