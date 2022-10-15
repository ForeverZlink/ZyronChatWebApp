using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Models
{
    public class Messages:IdentityUser
    {
        [Key]
        public int Id { get; set; }

        public string Sender { get; set; }
        public string Message { get; set; }
    }
}
