using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ZyronChatWebApp.Models;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using System.Security.Claims;

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
            if (User.Identity.IsAuthenticated) {
                var IdUserPrivate = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var IdUserPublic = this.Context.UserPublic.FirstOrDefault(x => x.IdPrivate == IdUserPrivate).IdPublic; 
                
                var ChatsOrdelyMoreRecentToMoreOlder = this.ChatMessagesLogic.OrderChatsByRecentMessages(IdUserPublic);

                if (ChatsOrdelyMoreRecentToMoreOlder != null)
                {

                    return View(ChatsOrdelyMoreRecentToMoreOlder);
                }





            }
            
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