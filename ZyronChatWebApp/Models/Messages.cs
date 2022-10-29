using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Models
{
    public class Messages
    {
        [Key]
        public string Id { get; set; }

        public string Sender { get; set; }

        public DateTime DateSended { get; set; }
        public TimeSpan TimeSended { get; set; }
        public string Message { get; set; }


        
        public string ChatMessagesId { get; set; }
        public ChatMessages ChatMessages { get; set; }

    }
}
