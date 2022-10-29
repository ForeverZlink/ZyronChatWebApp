using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Models
{
    public class UserModelCustom:IdentityUser
    {
         public UserScheduleListOfContacts UserScheduleListOfContacts { get; set; }

        [InverseProperty("UserSender")]
        public ICollection<ChatMessages> UsersSender { get; set; }

        [InverseProperty("UserReceiver")]
        public ICollection<ChatMessages> UserReceiver { get; set; }
    }
}
