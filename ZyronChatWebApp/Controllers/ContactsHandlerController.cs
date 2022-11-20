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

using Serilog;
using Serilog.Events;

namespace ZyronChatWebApp.Controllers
{
    public class ContactsHandlerController : Controller
    {
        public UserContext Context { get; set; }
        public ChatMessagesLogic ChatMessagesLogic { get; set; }
        
       
        public UserScheduleListOfContactsLogic UserListOfContactsLogic { get; set; }
        public ContactsHandlerController(UserContext dbContext)
        {
            this.Context = dbContext;
            this.ChatMessagesLogic = new ChatMessagesLogic(dbContext);
            this.UserListOfContactsLogic = new UserScheduleListOfContactsLogic(dbContext);
        }


        public async Task<IActionResult> Index()
        {

            //This action has the mission of show the user alls contacts that he have 
            //At this way, all contacts of the user are diplayed.
            Log.Information("Verifying if the user is logged");
            if (User.Identity.IsAuthenticated) {
                Log.Information("User is logged");

                Log.Information("Getting user object ");
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                Log.Information("Getting the list of contact of the users ");
                var listOfContacts = this.Context.UserScheduleListOfContacts.Include("ContactsInformations").FirstOrDefault(x => x.UserId == user.Id);

                Log.Information("Verifying if the list of contact are valid");
                if (listOfContacts != null)
                {
                    Log.Information("Sucess, list of contact is valid");

                    Log.Information("Returning the view");
                    return View(listOfContacts);
                }
                
                
            }
            Log.Information("User not is authenticated ");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewContact(string NameOfContact, string Surname)
        {

            Log.Information("Starting controller",DateTime.UtcNow);
            Log.Information("Verifying if the user is logged", DateTime.UtcNow);
            if (User.Identity.IsAuthenticated)
            {
                Log.Information("User is logged", DateTime.UtcNow);

                Log.Information("Getting user caller username", DateTime.UtcNow);
                var username = User.FindFirstValue(ClaimTypes.Name);

                Log.Information("Searching user caller in database ", DateTime.UtcNow);
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);

                Log.Information("Searching user to send in databse ", DateTime.UtcNow);
                var userToAdd = this.Context.Users.FirstOrDefault(x => x.UserName == NameOfContact);

                Log.Information("Verifyning if both exists", DateTime.UtcNow);
                if (user == null || userToAdd==null)
                {
                    Log.Information("One user, or both are not registered", DateTime.UtcNow);
                    return NotFound();
                }
                else
                {
                    Log.Information("Both users exists");

                    Log.Information("Trying create a new contact to relate with the user caller", DateTime.UtcNow);
                    var Sucess =this.UserListOfContactsLogic.AddNewContact(user.Id, NameOfContact, Surname);
                    Log.Information("End of the process of add the contact", DateTime.UtcNow);

                    Log.Information("Verifying if the contact was added", DateTime.UtcNow);
                    if (Sucess == false) {
                        Log.Information("Contact not added with sucess", DateTime.UtcNow);
                        return NotFound();
                    }

                    Log.Information("Creating a new Chat between if contact and the user", DateTime.UtcNow);

                    bool ChatCreatedWithSucess= this.ChatMessagesLogic.CreateNewChat(user.Id, userToAdd.Id);
                    Log.Information("End of the operation of creating a new chat ", DateTime.UtcNow);

                    Log.Information("Verifying if was sucess", DateTime.UtcNow);
                    if (ChatCreatedWithSucess)
                    {
                        Log.Information("Operation was the sucess. Not errors found", DateTime.UtcNow);

                        Log.Information("Setting propertys", DateTime.UtcNow);
                        ViewBag.ContactAddedWithSucess = true;

                        Log.Information("Returning the view ", DateTime.Now);
                        return View();
                    }
                    else
                    {
                        Log.Information("Error, wasnt possible create the object of the type Chat");
                        ViewBag.ContactAlreadyExists = true;
                    }

                    
                }
                    
                
            }
            Log.Information("Returning not found", DateTime.UtcNow);
            return NotFound();


        }
            




    }
}
