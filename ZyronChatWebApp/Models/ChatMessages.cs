using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZyronChatWebApp.Models
{
    public class ChatMessages 
    {
        [Key]
        public string Id { get; set; }


        public string IdUserReceiver { get; set; }
        public string IdUserSender { get; set; }

        
        public  UserModelCustom UserReceiver{ get; set; }

        public  UserModelCustom UserSender { get; set; }

        public  ICollection<Messages> MessagesList { get; set; }

            
    }
}
