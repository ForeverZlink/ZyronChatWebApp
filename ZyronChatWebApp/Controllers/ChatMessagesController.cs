using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ChatMessagesController : Controller
    {
        private readonly ILogger<ChatMessagesController> _logger;

        public ChatMessagesController(ILogger<ChatMessagesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChatWithASpecificUser(string UserToSendAMessageIdentification)
        {
            return View();
        }
    }
}