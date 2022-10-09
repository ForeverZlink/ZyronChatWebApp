using Microsoft.AspNetCore.Identity;

namespace ZyronChatWebApp.Models
{
    public class UserModelCustom:IdentityUser
    {
        public UserScheduleListOfContacts ListOfContacts { get; set; }
    }
}
