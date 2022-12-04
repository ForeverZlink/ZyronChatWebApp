using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZyronChatWebApp.Models;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;

namespace ZyronChatWebApp.Controllers
{
    public class DashManagementController : Controller
    {
        private readonly ILogger<DashManagementController> _logger;

        public UserContext Context { get; set; }
        public ChatMessagesLogic ChatMessagesLogic { get; set; }

        public DashManagementController(UserContext dbcontext, ILogger<DashManagementController> logger)
        {
            this.Context = dbcontext;
            this.ChatMessagesLogic = new ChatMessagesLogic(dbcontext);
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}