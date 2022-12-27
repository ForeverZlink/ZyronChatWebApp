using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZyronChatWebApp.Models
{
    public class Notifications 
    {
        [Key]
        public string Id { get; set; }


        public string IdUserCreatorOfNotification { get; set; }
        public UserPublic UserCreatorOfNotification { get; set; }

        public  ICollection<Messages> MessagesList { get; set; }

            
    }
}
