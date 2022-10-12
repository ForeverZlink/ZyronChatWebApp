using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZyronChatWebApp.Models
{
    public class ContactInformations 
    {
        [Key]
        public int Id { get; set; }

        //Name of the user registered in program.
        public string UsernameOfIdentification { get; set; }
        public string Surname { get; set; }

        public int IdUserScheduleListOfContacts { get; set; }
        public UserScheduleListOfContacts? UserScheduleListOfContacts { get; set; }

            
    }
}
