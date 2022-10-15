using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZyronChatWebApp.Models
{
    public class ChatMessages 
    {
        [Key]
        public int Id { get; set; }

        public int IdUserSender { get; set; }
        public UserModelCustom UserSender { get; set; }

        public int IdUserReceiver { get; set; }
        public UserModelCustom UserReceiver { get; set; }

            
    }
}
