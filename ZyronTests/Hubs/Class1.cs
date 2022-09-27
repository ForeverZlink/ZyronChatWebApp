using Microsoft.AspNetCore.Identity;
using ZyronChatWebApp.Controllers;
using ZyronChatWebApp.Controllers.Account;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;
using Moq;

namespace ZyronTests.Controllers
{
    public class Chat
    {
        

        static public UserContext context = new DatabaseConstructorTesting().CreateContext();


        [Fact]
        public void SendMessage()
        {
          
        }
    }
}