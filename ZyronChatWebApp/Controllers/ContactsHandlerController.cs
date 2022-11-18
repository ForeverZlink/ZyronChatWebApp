using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Xml.Linq;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ContactsHandlerController : Controller
    {
        public UserContext Context { get; set; }
        public ChatMessagesLogic ChatMessagesLogic { get; set; }
        public ILogger<ContactsHandlerController> Logger { get; set; }
        public UserScheduleListOfContactsLogic UserListOfContactsLogic { get; set; }
        public ContactsHandlerController(UserContext dbContext, ILogger<ContactsHandlerController> logger)
        {
            this.Context = dbContext;
            this.ChatMessagesLogic =  new ChatMessagesLogic(dbContext);
            this.Logger = logger;
            this.UserListOfContactsLogic = new UserScheduleListOfContactsLogic(dbContext);
        }


        public async Task<IActionResult> Index()
        {
            //This action has the mission of show the user alls contacts that he have 
            //At this way, all contacts of the user are diplayed.
            if (User.Identity.IsAuthenticated) {
            
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                var listOfContacts = this.Context.UserScheduleListOfContacts.Include("ContactsInformations").FirstOrDefault(x => x.UserId == user.Id);
                return View(listOfContacts);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewContact(string NameOfContact, string Surname)
        {
            if (User.Identity.IsAuthenticated)
            {
                
                var username = User.FindFirstValue(ClaimTypes.Name);
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);
                var userToAdd = this.Context.Users.FirstOrDefault(x => x.UserName == NameOfContact);
                
                if (user == null || userToAdd==null)
                {
                    return NotFound();
                }
                else
                {
                    var Sucess =this.UserListOfContactsLogic.AddNewContact(user.Id, NameOfContact, Surname);
                    if (Sucess == false) {
                        return NotFound();
                    }

                    bool ChatCreatedWithSucess= this.ChatMessagesLogic.CreateNewChat(user.Id, userToAdd.Id);
                    if (ChatCreatedWithSucess)
                    {
                        ViewBag.ContactAddedWithSucess = true;
                        return View();
                    }
                    else
                    {
                        ViewBag.ContactAlreadyExists = true;
                    }

                    
                }
                    
                
            }
            return NotFound();


        }
            




    }
}
