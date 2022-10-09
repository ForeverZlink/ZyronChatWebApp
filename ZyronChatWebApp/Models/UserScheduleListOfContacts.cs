using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZyronChatWebApp.Models
{
    public class UserScheduleListOfContacts 
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserModelCustom User { get; set; }

        public ICollection<ContactInformations> ContactsInformations { get; set; }
            
    }
}
