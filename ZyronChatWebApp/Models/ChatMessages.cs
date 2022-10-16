using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZyronChatWebApp.Models
{
    public class ChatMessages 
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserModelCustom")]
        public string IdUserSender { get; set; }
        public UserModelCustom UserSender { get; set; }


        [ForeignKey("UserModelCustom")]
        public string IdUserReceiver { get; set; }
        public UserModelCustom UserReceiver { get; set; }

        public ICollection<Messages> MessagesList { get; set; }

            
    }
}
