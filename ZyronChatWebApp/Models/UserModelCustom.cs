using Microsoft.AspNetCore.Identity;

using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Models
{
    public class UserModelCustom:IdentityUser
    {
      public UserScheduleListOfContacts UserScheduleListOfContacts { get; set; }
    }
}
