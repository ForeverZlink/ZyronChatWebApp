using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Models
{
    public class UserPublic
    {
        //This class has the purpose of increase the security 
        //Always when its necessary return a user information
        // not will be returned all private informations of the other User in database 

        //Of this manner, just the essencial information its passed to the final user.
        //In this context, when its very easy get a user of this to the layout of customer. 

        [Key]
        public string IdPublic { get; set; }

        //Id related with a the user model custom object id
        public string IdPrivate { get; set; }
        public string Username { get; set; }

        public UserScheduleListOfContacts UserScheduleListOfContacts { get; set; }
        public Notifications Notifications {get;set;}

        [InverseProperty("UserSender")]
        public ICollection<ChatMessages> UsersSender { get; set; }

        [InverseProperty("UserReceiver")]
        public ICollection<ChatMessages> UserReceiver { get; set; }
    }
}
