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
            this.Logger.LogInformation("Verifying if the user is logged");
            if (User.Identity.IsAuthenticated) {
                this.Logger.LogInformation("User is logged");
            
                this.Logger.LogInformation("Getting user object ");
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                this.Logger.LogInformation("Getting the list of contact of the users ");
                var listOfContacts = this.Context.UserScheduleListOfContacts.Include("ContactsInformations").FirstOrDefault(x => x.UserId == user.Id);

                this.Logger.LogInformation("Verifying if the list of contact are valid");
                if (listOfContacts != null)
                {
                    this.Logger.LogInformation("Sucess, list of contact is valid");

                    this.Logger.LogInformation("Returning the view");
                return View(listOfContacts);
            }
                
                
            }
            this.Logger.LogInformation("User not is authenticated ");
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
